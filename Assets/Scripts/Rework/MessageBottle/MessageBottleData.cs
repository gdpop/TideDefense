namespace TideDefense
{
    using UnityEngine;
    using System;

    [CreateAssetMenu(
        fileName = "MessageBottleData",
        menuName = "TideDefense/MessageBottleData",
        order = 0
    )]
    public class MessageBottleData : ScriptableObject
    {
		#region Fields

        [SerializeField]
        private MessageBottleType _type = MessageBottleType.None;
        public MessageBottleType type
        {
            get { return _type; }
        }

        [SerializeField, TextArea(3, 20)]
        private string _text = "";
        public string text
        {
            get { return _text; }
        }

		#endregion

		#region Methods

		#endregion
    }
}
