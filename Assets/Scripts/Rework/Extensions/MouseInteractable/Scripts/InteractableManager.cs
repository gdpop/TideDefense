namespace PierreMizzi.MouseInteractable
{
    using UnityEngine;

    public class InteractableManager
    {
        private Camera _camera = null;

        // TODO : Use LayerMask for raycasting
        private LayerMask _interactableLayerMask;

        private IClickable _currentClickable;

        private IHoverable _raycastedHoverable;
        private IHoverable _currentHoverable;

        public const int MOUSE_LEFT = 0;
        public const int MOUSE_RIGHT = 1;

        #region Long Click

        private HoldClickSetting _leftHoldClickSetting = null;

        public HoldClickable _currentHoldClickable = null;

        #endregion

        public InteractableManager()
        {
            _camera = Camera.main;
            _leftHoldClickSetting = new HoldClickSetting(MOUSE_LEFT, 1f, 3f);
        }

        public void Update()
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Manage Clickable Interactions
                if (hit.transform.TryGetComponent<IClickable>(out _currentClickable))
                    ManageClickable(hit, _currentClickable);
                else if (_currentClickable != null)
                    _currentClickable = null;

                // Manage HoldClickable
                if (hit.transform.TryGetComponent<HoldClickable>(out _currentHoldClickable))
                {
                    ManageHoldClickable(_currentHoldClickable, _leftHoldClickSetting);
                }
                else if (_currentHoldClickable != null)
                {
                    ManageLeavingHoldClickable(_leftHoldClickSetting);
                    _currentHoldClickable = null;
                }

                // Manage Hoverable
                if (hit.transform.TryGetComponent<IHoverable>(out _raycastedHoverable))
                {
                    if (_raycastedHoverable.isHoverable)
                        ManageHoverable(hit);
                    // Hoverable suddenly become non-hoverable while being raycasted, so we stop hovering it
                    else if (_raycastedHoverable.isHovered)
                        ForceExitHoverable();
                }
            }
            else if (_currentHoverable != null)
            {
                ForceExitHoverable();
            }
            else if (_currentHoldClickable != null)
            {
                ManageLeavingHoldClickable(_leftHoldClickSetting);
                _currentHoldClickable = null;
            }
        }

        #region IClickable

        public void ManageClickable(RaycastHit hit, IClickable clickable)
        {
            if (Input.GetMouseButtonDown(MOUSE_LEFT))
            {
                if (clickable.isClickable)
                    clickable.OnLeftClick(hit);
            }
        }

        #endregion

        #region HoldClickable

        public void ManageHoldClickable(HoldClickable interactable, HoldClickSetting setting)
        {
            // Checks if mouse is down for the given mouseButton
            if (Input.GetMouseButton(setting.mouseButtonID))
            {
                // Assign the newly clicked HoldClickable object
                if (setting.currentHoldClickable == null)
                    setting.currentHoldClickable = interactable;

                HoldClickStatus immediateStatus = setting.currentStatus;

                setting.currentHoldTime += Time.deltaTime;

                // Here we check is the status changed this frame, and then fire the event according to the new status
                if (immediateStatus != setting.currentStatus)
                {
                    // Debug.Log($"CHANGED STATE {setting.currentStatus} : {setting.currentHoldTime}");
                    switch (setting.currentStatus)
                    {
                        case HoldClickStatus.inLong:
                            setting.InvokeStartHoldClick();
                            break;
                        case HoldClickStatus.completed:
                            setting.InvokeCompleteHoldClick();
                            break;
                    }
                }

                // If it's being held, we fire the Progress event
                if (setting.currentStatus == HoldClickStatus.inLong)
                    setting.InvokeProgressHoldClick();
            }
            else
            {
                if (setting.currentStatus == HoldClickStatus.inLong)
                    setting.InvokeCancelHoldClick();

                setting.currentHoldTime = 0;
            }
        }

        public void ManageLeavingHoldClickable(HoldClickSetting setting)
        {
            if (setting.currentHoldClickable != null)
            {
                if (setting.currentStatus == HoldClickStatus.inLong)
                    setting.InvokeCancelHoldClick();

                setting.currentHoldTime = 0f;
                setting.currentHoldClickable = null;
            }
        }

            
        #endregion


        #region Hoverable

        public void ManageHoverable(RaycastHit hit)
        {
            // No current hoverable, we set it
            if (_currentHoverable == null)
            {
                _currentHoverable = _raycastedHoverable;
                _currentHoverable.OnHoverEnter(hit);
                _currentHoverable.OnHover(hit);
            }
            // We're hovering the same IHoverable we previously raycasted
            else if (_currentHoverable == _raycastedHoverable)
            {
                _currentHoverable.OnHover(hit);
            }
            // We raycasted another IHoverable
            else if (_currentHoverable != _raycastedHoverable)
            {
                _currentHoverable.OnHoverExit();

                _currentHoverable = _raycastedHoverable;
                _currentHoverable.OnHoverEnter(hit);
                _currentHoverable.OnHover(hit);
            }
        }

        /// <summary>
        /// Here we stop volontarily to hover the _currentHoverable
        /// </summary>
        private void ForceExitHoverable()
        {
            _currentHoverable.OnHoverExit();
            _currentHoverable = null;

            _raycastedHoverable = null;
        }

        #endregion
    }
}
