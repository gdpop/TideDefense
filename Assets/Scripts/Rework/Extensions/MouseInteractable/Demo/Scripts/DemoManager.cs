namespace PierreMizzi.MouseInteractable
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DemoManager : MonoBehaviour
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
