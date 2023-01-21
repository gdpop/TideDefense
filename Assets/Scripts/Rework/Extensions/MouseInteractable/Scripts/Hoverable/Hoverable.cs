namespace PierreMizzi.MouseInteractable
{
	using System;
	using UnityEngine;

    public class Hoverable : MonoBehaviour
	{

		[SerializeField] private bool _isInteractable = true;
		public bool isInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

		[SerializeField] private bool _isHovered = false;
		public bool isHovered { get { return _isHovered; } set { _isHovered = value; } }

		private void Awake() {

			onHoverEnter = (RaycastHit hit)=>{};
			onHoverExit = ()=>{};
			onHover = (RaycastHit hit)=>{};
		}

        public RaycastDelegate onHoverEnter = null;
        public Action onHoverExit;
        public RaycastDelegate onHover = null;
		
	}
}