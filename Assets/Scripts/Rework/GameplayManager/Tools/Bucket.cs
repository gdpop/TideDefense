using PierreMizzi.MouseInteractable;
using UnityEngine;
using DG.Tweening;

namespace TideDefense
{

    public class Bucket : MonoBehaviour, IClickable, IHoverable
    {

		#region Fields

		#region Tool parent class ?

		protected bool _isGrabbed = false;
		protected ToolStatus _status = ToolStatus.Dropped;

		#endregion

		[SerializeField] private GameplayChannel _gameplayChannel = null;

		#region Hover Animation

		[Header("Hover Animation")]
		[SerializeField] private float _hoverDuration = 0.1f;
		[SerializeField] private float _hoverYOffset = 0.1f;

		private Vector3 _currentPosition = new Vector3();
		private Vector3 _hoveredPosition = new Vector3();

		private Tween _hoverTween = null;

		#endregion

		#region Grabbed Behaviour

		private BoxCollider _boxCollider = null;

		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour

		private void Awake() {
			_boxCollider = GetComponent<BoxCollider>();
		}

		private void Start() {
			SetDropped();

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
			_boxCollider.enabled = false;
		}

		public virtual void SetDropped()
		{
			_status = ToolStatus.Dropped;
			_boxCollider.enabled = true;
			transform.rotation = Quaternion.identity;

			_currentPosition = transform.position;
			_hoveredPosition = transform.position + new Vector3(0f, _hoverYOffset, 0f);
		}

		#endregion

		#endregion

		#region IClickable

		public void OnLeftClick(RaycastHit hit)
		{
			if(!_isInteractable)
				return;

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
			if(!_isInteractable)
				return;
		}

		public void OnHoverEnter(RaycastHit hit)
		{
			Debug.Log("OnHoverEnter");
			if(!_isInteractable)
				return;

			_isHovered = true; 
			if(!_isGrabbed)
			{
				_hoverTween.Kill();
				_hoverTween = transform.DOMove(_hoveredPosition, _hoverDuration).SetEase(Ease.InCubic);
			}
		}

		public void OnHoverExit()
		{
			Debug.Log("OnHoverExit");
			if(!_isInteractable)
				return;

			_isHovered = false;
			if(!_isGrabbed)
			{
				_hoverTween.Kill();
				_hoverTween = transform.DOMove(_currentPosition, _hoverDuration).SetEase(Ease.OutCubic);
			}
		}
			
		#endregion

	}
}
