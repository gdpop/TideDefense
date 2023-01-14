namespace PierreMizzi.MouseInteractable
{
    using UnityEngine;

    public class MouseInteractableManager
    {
        private Camera _camera = null;

        private LayerMask _interactableLayerMask;

        public MouseInteractableManager()
        {
            _camera = Camera.main;
        }

        public void Update()
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                Debug.Log(objectHit.name);
            }
        }
    }
}
