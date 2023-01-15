namespace PierreMizzi.Collisions
{
    using UnityEngine;
    using TideDefense;

    public class TriggerInteractable : MonoBehaviour
    {
        public ColliderDelegate onTriggerEnter = null;

        private void OnEnable()
        {
            Initialize();
        }

        public void Initialize()
        {
            onTriggerEnter = (Collider other) => { };
        }

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Invoke(other);
        }

        private void OnTriggerExit(Collider other) { }
    }
}
