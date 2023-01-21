namespace PierreMizzi.MouseInteractable
{
    using UnityEngine;

	public class DemoClickable : MonoBehaviour
	{
		[SerializeField] private Clickable _clickable = null;

		private void Start() {
			_clickable.OnLeftClick += CallbackOnLeftClick;
		}

		private void CallbackOnLeftClick(RaycastHit hit)
		{
			Debug.Log("CallbackOnLeftClick");
		}
	}
}
