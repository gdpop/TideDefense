namespace PierreMizzi.MouseInteractable
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DemoManager : MonoBehaviour
    {
        private InteractableManager _interactableManager = null;

        [SerializeField]
        private LayerMask _layerMask;

        private void Start()
        {
            _interactableManager = new InteractableManager(_layerMask);
        }

        private void Update()
        {
            _interactableManager.Update();
        }
    }
}
