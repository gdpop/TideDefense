namespace TideDefense
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public class MessageBottlePanel : MonoBehaviour
    {
		#region Fields

        private UIDocument _document = null;

		// Background
        private const string BACKGROUND_NAME = "Background";
        private VisualElement _background = null;

		// Message
        private const string MESSAGE_NAME = "Message";
        private VisualElement _message = null;

		// Close Button
        private const string CLOSE_BUTTON_NAME = "CloseButton";
        private VisualElement _closeButton = null;

		#endregion

		#region Methods

		#region MonoBehaviour

        private void Start()
        {
            _document = GetComponent<UIDocument>();
            Initiliaze();
        }

		#endregion

        public void Initiliaze()
        {
            _background = _document.rootVisualElement.Query<VisualElement>(BACKGROUND_NAME);
            _message = _document.rootVisualElement.Query<VisualElement>(MESSAGE_NAME);
            _closeButton = _document.rootVisualElement.Query<VisualElement>(CLOSE_BUTTON_NAME);
        }

		#endregion
    }
}
