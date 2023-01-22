namespace TideDefense
{
    using System;
	using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;
    using System.Linq;

    // - Idle
    // Left Click - Grab tool

    // - Shovel
    // Left Click - Drop Tool
    // Left Click - Fill bucket

    // - Bucket
    // Left Click - Drop Tool
    // Hold Left Click - Build rempart

    public delegate void ControlHintEvent(params ControlHintType[] type);

    public class ControlHintsPanel : MonoBehaviour
    {
		#region Fields

        [SerializeField] private UIChannel _UIChannel = null;

        [SerializeField]
        private UIDocument _document = null;

		private Dictionary<ControlHintType, VisualElement> _controlHintLabels = new Dictionary<ControlHintType, VisualElement>();

		#endregion

		#region Methods

        private void Start()
        {
            VisualElement root = _document.rootVisualElement;

            VisualElement grabToolButton = root.Q<VisualElement>("GrabTool");
            Debug.Log(grabToolButton != null);

            foreach (ControlHintType type in Enum.GetValues(typeof(ControlHintType)))
            {
                VisualElement element = root.Q<VisualElement>(type.ToString());

				if(element != null)
                    _controlHintLabels.Add(type, element);
            }

            DisplayControlHint(ControlHintType.None);

            if(_UIChannel != null)
                _UIChannel.onRefreshControlHints += DisplayControlHint;
        }

        private void OnDestroy() {
            if(_UIChannel != null)
                _UIChannel.onRefreshControlHints -= DisplayControlHint;
        }

        private void DisplayControlHint(params ControlHintType[] types)
        {
            foreach (KeyValuePair<ControlHintType, VisualElement> pair in _controlHintLabels)
                pair.Value.style.display = types.Contains(pair.Key) ? DisplayStyle.Flex : DisplayStyle.None;
        }

		#endregion
    }
}
