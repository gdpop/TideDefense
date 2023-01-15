namespace PierreMizzi.MouseInteractable
{
    using UnityEngine;

    public class InteractableManager
    {
        private Camera _camera = null;


        // TODO : Use LayerMask for raycasting
        private LayerMask _interactableLayerMask;

        private IClickable _currentClickable;

        private const int MOUSE_LEFT = 0;
        private const int MOUSE_RIGHT = 1;

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
                else if(_currentClickable != null)
                    _currentClickable = null;
            }
        }

        public void ManageClickable(RaycastHit hit, IClickable clickable)
        {
            if (Input.GetMouseButtonDown(MOUSE_LEFT))
            {
                clickable.onLeftClick(hit);
            }
        }
    }
}
