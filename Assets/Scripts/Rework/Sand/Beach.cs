namespace TideDefense
{
    using PierreMizzi.MouseInteractable;
    using UnityEngine;

    // TODO : Refact parameters of the Beach : BeachSlope, BeachBottom ...
    // TODO : Separate visual and logical elements

    public class Beach : MonoBehaviour, IClickable, IHoverable
    {
		#region Fields

        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

		#endregion

		#region Methods

		#endregion

		#region IClickable

        public void OnLeftClick(RaycastHit hit)
        {
            _gameplayChannel.onClickBeach.Invoke(hit);
        }

		#endregion

        #region IHoverable

        [SerializeField] private bool _isInteractable = true;
        public bool isInteractable { get { return _isInteractable; } set { _isInteractable = value; } }
        
        [SerializeField]
        private bool _isHovered = false;
        public bool isHovered
        {
            get { return _isHovered; }
            set { _isHovered = value; }
        }

        public void OnHoverEnter(RaycastHit hit)
        {
            isHovered = true;
        }

        public void OnHoverExit()
        {
            isHovered = false;
        }

        public void OnHover(RaycastHit hit)
        {
            _gameplayChannel.onHoverBeach.Invoke(hit);
        }

        #endregion
    }
}
