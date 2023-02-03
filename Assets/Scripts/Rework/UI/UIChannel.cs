namespace TideDefense
{
    using System;
    using UnityEngine;

    [CreateAssetMenu(fileName = "UIChannel", menuName = "ScriptableObjects/UIChannel", order = 1)]
    public class UIChannel : ScriptableObject
    {
		#region Control Hints


        public Action onHideAllControlHint = null;
        public ControlHintEvent onDisplayControlHint = null;
        public ControlHintEvent onHideControlHint = null;

		#endregion

		#region Message Bottle

        public FloatingMessageBottleDelegate onDisplayMessageBottle = null;
        public Action onHideMessageBottle = null;

		#endregion

        private void OnEnable()
        {
            // Control Hints
            onHideAllControlHint = null;
            onDisplayControlHint = (ControlHintType[] type) => { };
            onHideControlHint = (ControlHintType[] type) => { };

            // Message Bottle
            onDisplayMessageBottle = (MessageBottleData data) => { };
            onHideMessageBottle = () => { };
        }
    }
}
