namespace TideDefense
{
    using UnityEngine;
    using PierreMizzi.MouseInteractable;

    public class Application : MonoBehaviour
    {
        private InteractableManager _interactableManager = null;

        private void Start()
        {
            _interactableManager = new InteractableManager();
        }

        private void Update()
        {
            _interactableManager.Update();
        }
    }
}
