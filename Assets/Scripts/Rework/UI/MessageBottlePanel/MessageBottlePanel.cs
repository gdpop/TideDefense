namespace TideDefense
{
    using UnityEngine;
    using UnityEngine.UIElements;
    using CodesmithWorkshop.UIToolkit;
    using DG.Tweening;

    public class MessageBottlePanel : MonoBehaviour
    {
		#region Fields

        private UIDocument _document = null;

        [SerializeField]
        private UIChannel _UIChannel = null;

        // Background
        private const string BACKGROUND_NAME = "Background";
        private VisualElement _background = null;
        private const string FADE_IN_OUT = "background-fade-in-out";

        // Message Container
        private const string MESSAGE_CONTAINER_NAME = "MessageContainer";
        private VisualElement _messageContainer = null;
        private const string MESSAGE_CONTAINER_ANIM = "message-container";

        // Text Label
        private const string TEXT_LABEL_NAME = "TextLabel";
        private Label _textLabel = null;

        // Close Button
        private const string CLOSE_BUTTON_NAME = "CloseButton";
        private VisualElement _closeButton = null;
        private const string CLOSE_BUTTON_DISPLAY_HIDE = "close-button-display-hide";

		#endregion

		#region Methods

		#region MonoBehaviour

        private void Start()
        {
            if (_UIChannel != null)
                _UIChannel.onDisplayMessageBottle += Display;

            _document = GetComponent<UIDocument>();
            Initiliaze();
        }

        private void OnDestroy()
        {
            if (_UIChannel != null)
                _UIChannel.onDisplayMessageBottle -= Display;
        }

		#endregion

        public void Initiliaze()
        {
            _background = _document.rootVisualElement.Query<VisualElement>(BACKGROUND_NAME);
            _messageContainer = _document.rootVisualElement.Query<VisualElement>(MESSAGE_CONTAINER_NAME);
            _textLabel = _document.rootVisualElement.Query<Label>(TEXT_LABEL_NAME);            
            _closeButton = _document.rootVisualElement.Query<VisualElement>(CLOSE_BUTTON_NAME);
            _closeButton.RegisterCallback<ClickEvent>(CallbackCloseButton);

            _background.AddToClassList(FADE_IN_OUT);

            _messageContainer.AddToClassList(MESSAGE_CONTAINER_ANIM);

            _closeButton.AddToClassList(CLOSE_BUTTON_DISPLAY_HIDE);
        }

        public void Display(MessageBottleData data)
        {
            _textLabel.text = data.text;
            Display();
        }

        [ContextMenu("Display")]
        public void Display()
        {
            UIHelpers.DelayAddToClassList(_background);
            UIHelpers.DelayAddToClassList(_messageContainer);

            DOVirtual.DelayedCall(3, DisplayCloseButton);
        }

        public void DisplayCloseButton()
        {
            UIHelpers.DelayAddToClassList(_closeButton);
        }

        public void CallbackCloseButton(ClickEvent clickEvent)
        {
            Hide();
        }

        [ContextMenu("Hide")]
        public void Hide()
        {
            UIHelpers.DelayAddToClassList(_background);
            UIHelpers.DelayAddToClassList(_messageContainer);
            UIHelpers.DelayAddToClassList(_closeButton);
        }

		#endregion
    }
}
