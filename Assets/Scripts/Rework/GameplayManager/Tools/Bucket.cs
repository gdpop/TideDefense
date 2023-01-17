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
        public ToolStatus status = ToolStatus.Dropped;

		/// <summary> 
		/// Current Grid Cell the tool is dropped on
		/// </summary>
		[HideInInspector]
        public GridCell currentGridCell = null;


		#endregion

        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

        /// <summary>
        ///	Box Collider used to check hover and click
        /// </summary>
        [SerializeField]
        private BoxCollider _interactableBoxCollider = null;

		#region Hover Animation

        [Header("Hover Animation")]
        [SerializeField]
        private float _hoverDuration = 0.1f;

        [SerializeField]
        private float _hoverYOffset = 0.1f;

        private Vector3 _currentPosition = new Vector3();
        private Vector3 _hoveredPosition = new Vector3();

        private Tween _hoverTween = null;

		#endregion

		#region Grabbed Behaviour

        /// <summary>
        ///	This box collider is not used, it's only for the swaying effect when grabbed
        /// </summary>
        [SerializeField]
        private BoxCollider _grabBoxCollider = null;

		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour


        private void Start()
        {
            if (_gameplayChannel != null)
                _gameplayChannel.onChangeTool += CallbackOnChangeTool;
        }

        private void OnDestroy()
        {
            if (_gameplayChannel != null)
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
            status = ToolStatus.Grabbed;
			currentGridCell = null;

            _interactableBoxCollider.enabled = false;
            _grabBoxCollider.enabled = true;
        }

        public virtual void SetDropped(GridCell gridCell)
        {
            status = ToolStatus.Dropped;
			currentGridCell = gridCell;

            _interactableBoxCollider.enabled = true;
            _grabBoxCollider.enabled = false;

            transform.rotation = Quaternion.identity;

            // Used by hover animation
            _currentPosition = transform.position;
            _hoveredPosition = transform.position + new Vector3(0f, _hoverYOffset, 0f);
        }

		#endregion

		#endregion

		#region IClickable

        public void OnLeftClick(RaycastHit hit)
        {
            if (!_isInteractable)
                return;

            _gameplayChannel.onClickBucket.Invoke();
        }

		#endregion

		#region IHoverable

        [SerializeField]
        private bool _isInteractable = true;
        public bool isInteractable
        {
            get { return _isInteractable; }
            set { _isInteractable = value; }
        }

        [SerializeField]
        private bool _isHovered = false;
        public bool isHovered
        {
            get { return _isHovered; }
            set { _isHovered = value; }
        }

        public void OnHover(RaycastHit hit)
        {
            if (!_isInteractable)
                return;
        }

        public void OnHoverEnter(RaycastHit hit)
        {
            if (!_isInteractable)
                return;

            // Bucket makes a little jump above the ground
            _isHovered = true;
            if (!_isGrabbed)
            {
                _hoverTween.Kill();
                _hoverTween = transform
                    .DOMove(_hoveredPosition, _hoverDuration)
                    .SetEase(Ease.InCubic);
            }
        }

        public void OnHoverExit()
        {
            if (!_isInteractable)
                return;

            // Bucket goes back to position if not clicked
            _isHovered = false;
            if (!_isGrabbed)
            {
                _hoverTween.Kill();
                _hoverTween = transform
                    .DOMove(_currentPosition, _hoverDuration)
                    .SetEase(Ease.OutCubic);
            }
        }

		#endregion
    }
}
