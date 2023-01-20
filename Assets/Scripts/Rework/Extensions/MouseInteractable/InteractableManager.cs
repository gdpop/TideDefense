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

        private const int MOUSE_LEFT = 0;
        private const int MOUSE_RIGHT = 1;

        #region Long Click

        public ILongClickable _currentLongClickable = null;

        public const float _clickHoldTreshold = 1f;
        public const float _clickHoldDuration = 3f;

        public float _currentClickTime = 0f;

        public bool _hasStartedLongClick = false;

        public ClickStatus _currentClickStatus
        {
            get { return GetClickStatusFromClickTime(_currentClickTime); }
        }

        public enum ClickStatus
        {
            None,
            inTreshold,
            inLong,
            completed,
        }

        #endregion

        public InteractableManager()
        {
            _camera = Camera.main;
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

                // Debug.Log($"Check CurrentHoverable {_currentHoverable != null}");

                // Manage Long Clickable
                if (hit.transform.TryGetComponent<ILongClickable>(out _currentLongClickable))
                    ManageLongClickable(hit, _currentLongClickable);
                else if (_currentLongClickable != null)
                    _currentLongClickable = null;

                // Manage Hoverable
                if (hit.transform.TryGetComponent<IHoverable>(out _raycastedHoverable))
                {
                    if (_raycastedHoverable.isHoverable)
                        ManageHoverable(hit);
                    // Hoverable suddenly become non-hoverable while being raycasted, so we stop hovering it
                    else if (_raycastedHoverable.isHovered)
                        ForceHoverExit();
                }
            }
            else if (_currentHoverable != null)
            {
                ForceHoverExit();
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

        #region LonClickable


        public void ManageLongClickable(RaycastHit hit, ILongClickable interactable)
        {
            if (Input.GetMouseButton(MOUSE_LEFT))
            {
                ClickStatus immediateStatus = GetClickStatusFromClickTime(_currentClickTime);
                _currentClickTime += Time.deltaTime;

                if (immediateStatus != _currentClickStatus)
                {
                    if (_currentClickStatus == ClickStatus.inLong)
                        interactable.OnStartLongLeftClick();
                    else if(_currentClickStatus == ClickStatus.completed)
                        interactable.OnCompleteLongLeftClick();
                }

                // if(_currentClickStatus == ClickStatus.inLong)
                    // interactable.OnProgressLongLeftClick();
            }
        }

        private ClickStatus GetClickStatusFromClickTime(float time)
        {
            if (0 <= _currentClickTime && _currentClickTime < _clickHoldTreshold)
                return ClickStatus.inTreshold;
            else if (
                _clickHoldTreshold <= _currentClickTime && _currentClickTime < _clickHoldTreshold
            )
                return ClickStatus.inLong;
            else if (_clickHoldTreshold < _clickHoldDuration)
                return ClickStatus.completed;
        }

        private float GetProgressFromClickTime(float time)
        {
            return  time / (_clickHoldDuration - _clickHoldTreshold);
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
        private void ForceHoverExit()
        {
            _currentHoverable.OnHoverExit();
            _currentHoverable = null;

            _raycastedHoverable = null;
        }

        #endregion
    }
}
