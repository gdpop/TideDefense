namespace PierreMizzi.MouseInteractable
{
    using UnityEngine;

	// public delegate void RaycastDelegate(RaycastHit hit); 

    public class Clickable : MonoBehaviour
	{
        [SerializeField]
        private bool _isInteractable = true;
        public bool isInteractable
        {
            get { return _isInteractable; }
            set { _isInteractable = value; }
        }

        public RaycastDelegate OnLeftClick;

		private void Awake() {
			OnLeftClick = (RaycastHit hit)=>{};
		}

	}
}