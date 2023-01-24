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
                {
                    _controlHintLabels.Add(type, element);
                    element.style.display = DisplayStyle.None;
                }
            }



            if(_UIChannel != null)
            {
                _UIChannel.onHideAllControlHint += CallbackHideAllControlHint;
                _UIChannel.onDisplayControlHint += CallbackDisplayControlHint;
                _UIChannel.onHideControlHint += CallbackHideControlHint;
            }
        }

        private void OnDestroy() {
            if(_UIChannel != null)
            {
                _UIChannel.onHideAllControlHint -= CallbackHideAllControlHint;
                _UIChannel.onDisplayControlHint -= CallbackDisplayControlHint;
                _UIChannel.onHideControlHint -= CallbackHideControlHint;

            }
                
        }

        private void CallbackHideAllControlHint()
        {
            foreach (KeyValuePair<ControlHintType,VisualElement> pair in _controlHintLabels)
            {
                    pair.Value.style.display = DisplayStyle.None;
            }
        }

        private void CallbackDisplayControlHint(params ControlHintType[] types)
        {
            foreach (ControlHintType type in types)
            {
                if(!_controlHintLabels.ContainsKey(type))
                    continue;
                VisualElement element = _controlHintLabels[type];

                if(element != null)
                    element.style.display = DisplayStyle.Flex;
            }
        }

        private void CallbackHideControlHint(params ControlHintType[] types)
        {
            

            foreach (ControlHintType type in types)
            {
                if(!_controlHintLabels.ContainsKey(type))
                    continue;
                VisualElement element = _controlHintLabels[type];

                if(element != null)
                    element.style.display = DisplayStyle.None;
            }
        }

		#endregion
    }
}
