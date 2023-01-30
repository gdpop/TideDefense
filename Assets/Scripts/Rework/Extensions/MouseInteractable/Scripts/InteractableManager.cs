namespace PierreMizzi.MouseInteractable
{
    using UnityEngine;

    public class InteractableManager
    {
        private Camera _camera = null;

        // TODO : Use LayerMask for raycasting
        private LayerMask _interactableLayerMask;

        private Hoverable _raycastedHoverable;
        private Hoverable _currentHoverable;

        public const int MOUSE_LEFT = 0;
        public const int MOUSE_RIGHT = 1;

        #region Click

        private HoldClickSetting _leftHoldClickSetting = null;

        public HoldClickable _currentHoldClickable = null;

        #endregion

        public InteractableManager()
        {
            _camera = Camera.main;
            _leftHoldClickSetting = new HoldClickSetting(MOUSE_LEFT, 0.5f, 1f);
            Debug.Log(_interactableLayerMask.value);
        }

        public InteractableManager(LayerMask interactableLayerMask)
        {
            _interactableLayerMask = interactableLayerMask;
            _camera = Camera.main;
            _leftHoldClickSetting = new HoldClickSetting(MOUSE_LEFT, 0.5f, 1f);
        }

        public void Update()
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, _interactableLayerMask))
            {
                // Manage HoldClickable
                {
                    if (hit.transform.TryGetComponent<HoldClickable>(out _currentHoldClickable))
                    {
                        // Check if it's interactable
                        if (_currentHoldClickable.isInteractable)
                            ManageHoldClickable(_currentHoldClickable, _leftHoldClickSetting, hit);
                        // If it's raycasted but no longer interactable, we leave it
                        else
                            ManageLeavingHoldClickable(_leftHoldClickSetting, hit);
                    }
                    // If something different has been raycasted, we leave it
                    else if (CheckValidHoldClickable(_currentHoldClickable))
                    {
                        ManageLeavingHoldClickable(_leftHoldClickSetting, hit);
                    }
                }

                // Manage Hoverable
                {
                    if (hit.transform.TryGetComponent<Hoverable>(out _raycastedHoverable))
                    {
                        if (_raycastedHoverable.isInteractable)
                            ManageHoverable(hit);
                        // Hoverable suddenly become non-hoverable while being raycasted, so we stop hovering it
                        else if (_raycastedHoverable.isHovered)
                            ForceExitHoverable();
                    }
                }
            }
            else if (_currentHoverable != null)
            {
                ForceExitHoverable();
            }
            // If nothing is being raycasted, we leave the current clicable
            else if (CheckValidHoldClickable(_currentHoldClickable))
            {
                ManageLeavingHoldClickable(_leftHoldClickSetting, new RaycastHit());
            }
        }

        #region HoldClickable

        public void ManageHoldClickable(
            HoldClickable clickable,
            HoldClickSetting setting,
            RaycastHit hit
        )
        {
            // Checks if mouse is down for the given mouseButton
            if (Input.GetMouseButton(setting.mouseButtonID))
            {
                // Assign the newly clicked HoldClickable object
                if (setting.currentHoldClickable == null)
                {
                    setting.SetClickable(clickable);
                    setting.InvokeOnMouseDown(hit);
                }

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
                ManageLeavingHoldClickable(setting, hit);
        }

        public void ManageLeavingHoldClickable(HoldClickSetting setting, RaycastHit hit)
        {
            if (setting.currentHoldClickable != null)
            {
                if (setting.currentStatus == HoldClickStatus.inClick)
                    setting.InvokeClick(hit);
                else if (setting.currentStatus == HoldClickStatus.inLong)
                    setting.InvokeCancelHoldClick();

                setting.currentHoldTime = 0f;
                setting.UnsetClickable();
            }
            _currentHoldClickable = null;
        }

        private bool CheckValidHoldClickable(HoldClickable clickable)
        {
            return (clickable != null && clickable.isInteractable);
        }

        #endregion


        #region Hoverable

        public void ManageHoverable(RaycastHit hit)
        {
            // No current hoverable, we set it
            if (_currentHoverable == null)
            {
                _currentHoverable = _raycastedHoverable;
                _currentHoverable.isHovered = true;
                _currentHoverable.onHoverEnter(hit);
                _currentHoverable.onHover(hit);
            }
            // We're hovering the same IHoverable we previously raycasted
            else if (_currentHoverable == _raycastedHoverable)
            {
                _currentHoverable.onHover(hit);
            }
            // We raycasted another IHoverable
            else if (_currentHoverable != _raycastedHoverable)
            {
                _currentHoverable.onHoverExit();
                _currentHoverable.isHovered = false;

                _currentHoverable = _raycastedHoverable;
                _currentHoverable.isHovered = true;
                _currentHoverable.onHoverEnter(hit);
                _currentHoverable.onHover(hit);
            }
        }

        /// <summary>
        /// Here we stop volontarily to hover the _currentHoverable
        /// </summary>
        private void ForceExitHoverable()
        {
            _currentHoverable.onHoverExit();
            _currentHoverable.isHovered = false;
            _currentHoverable = null;

            _raycastedHoverable = null;
        }

        #endregion
    }
}
