using PierreMizzi.MouseInteractable;
using UnityEngine;
using DG.Tweening;

namespace TideDefense
{

    public class Bucket : MonoBehaviour, IClickable, IHoverable
    {

		#region Fields

		#region Tool parent class ?

		protected ToolStatus _status = ToolStatus.Dropped;

		#endregion

		[SerializeField] private GameplayChannel _gameplayChannel = null;

		protected bool _isGrabbed = false;

		#region Hover Animation

		[Header("Hover Animation")]
		[SerializeField] private float _hoverDuration = 0.1f;
		[SerializeField] private float _hoverYOffset = 0.1f;

		private Tween _hoverTween = null;

		#endregion

		#region Grabbed Behaviour

		private Rigidbody _rigidBody = null;
		
		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour
			
		private void Start() {
			if(_gameplayChannel != null)
				_gameplayChannel.onChangeTool += CallbackOnChangeTool;
		}

		private void OnDestroy() {
			
			if(_gameplayChannel != null)
				_gameplayChannel.onChangeTool -= CallbackOnChangeTool;
		}

		#endregion

		#region Tool parent class ?
			
		private void CallbackOnChangeTool(ToolType toolType)
		{
			_isGrabbed = toolType == ToolType.Bucket;
		}

		public virtual void SetGrabbed()
		{
			_status = ToolStatus.Grabbed;
		}

		public virtual void SetDropped()
		{
			_status = ToolStatus.Dropped;
		}

		#endregion

		#endregion

		#region IClickable

		public void OnLeftClick(RaycastHit hit)
		{
			_gameplayChannel.onClickBucket.Invoke();
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

		public void OnHover(RaycastHit hit)
		{

		}

		public void OnHoverEnter(RaycastHit hit)
		{
			_isHovered = true; 
			if(!_isGrabbed)
			{
				_hoverTween = transform.DOMoveY(_hoverYOffset,_hoverDuration).SetEase(Ease.OutElastic);
			}
		}

		public void OnHoverExit()
		{
			if(!_isGrabbed)
			{
				_hoverTween.PlayBackwards();
			}
		}
			
		#endregion

	}
}
