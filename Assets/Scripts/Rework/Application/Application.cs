namespace TideDefense
{
    using UnityEngine;
    using PierreMizzi.MouseInteractable;

    public class Application : MonoBehaviour
    {
        private MouseInteractableManager _interactableManager = null;

        private void Start()
        {
            _interactableManager = new MouseInteractableManager();
        }

        private void Update()
        {
            _interactableManager.Update();
        }
    }
}
