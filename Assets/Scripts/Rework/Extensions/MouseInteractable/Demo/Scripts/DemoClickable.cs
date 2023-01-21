namespace PierreMizzi.MouseInteractable
{
    using UnityEngine;

	public class DemoClickable : MonoBehaviour,  IClickable
	{
		[SerializeField] private bool _isClickable = true;
		public bool isClickable { get { return _isClickable; } set { _isClickable = value; } }

		[SerializeField] private bool _isInteractable = true;
		public bool isInteractable { get { return _isInteractable; } set { _isInteractable = value; } }

		public void OnLeftClick(RaycastHit hit)
		{
			Debug.Log("OnLeftClick");
		}
	}
}
