namespace TideDefense
{
	using UnityEngine;
	
	[CreateAssetMenu(fileName = "UIChannel", menuName = "ScriptableObjects/UIChannel", order = 1)]
	public class UIChannel : ScriptableObject
	{

		public ControlHintEvent onRefreshControlHints = null;

		private void OnEnable() {
			onRefreshControlHints = (ControlHintType[] type)=>{};
		}

	}
}