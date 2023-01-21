namespace TideDefense
{
    using UnityEngine;
    using PierreMizzi.MouseInteractable;

    public class Application : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactableLayer;
        private InteractableManager _interactableManager = null;
    
        private void Start()
        {
            _interactableManager = new InteractableManager(_interactableLayer);
        }

        private void Update()
        {
            _interactableManager.Update();
        }
    }
}
