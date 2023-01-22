namespace TideDefense
{
	using System;
	using UnityEngine;
	
	[CreateAssetMenu(fileName = "UIChannel", menuName = "ScriptableObjects/UIChannel", order = 1)]
	public class UIChannel : ScriptableObject
	{

		public Action onHideAllControlHint = null;
		public ControlHintEvent onDisplayControlHint = null;
		public ControlHintEvent onHideControlHint = null;

		private void OnEnable() {
			onHideAllControlHint = null;
			onDisplayControlHint = (ControlHintType[] type)=>{};
			onHideControlHint = (ControlHintType[] type)=>{};
		}

	}
}