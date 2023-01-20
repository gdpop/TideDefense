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

        #region Click

        public const float _clickHoldTreshold = 0f;
        public const float _clickHoldDuration = 0f;

        

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


                // Manage Hoverable
                if (hit.transform.TryGetComponent<IHoverable>(out _raycastedHoverable))
                {
                    if(_raycastedHoverable.isHoverable)
                        ManageHoverable(hit);

                    // Hoverable suddenly become non-hoverable while being raycasted, so we stop hovering it
                    else if(_raycastedHoverable.isHovered)
                        ForceHoverExit();

                }
            }
            else if (_currentHoverable != null)
            {
                ForceHoverExit();
            }
        }

        public void ManageClickable(RaycastHit hit, IClickable clickable)
        {
            if (Input.GetMouseButtonDown(MOUSE_LEFT))
            {
                if(clickable.isClickable)
                    clickable.OnLeftClick(hit);
            }
        }

        public void ManageClickableWithHolding(RaycastHit hit, IClickable clickable)
        {

        }

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

    }
}
