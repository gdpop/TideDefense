using PierreMizzi.MouseInteractable;
using UnityEngine;
using DG.Tweening;
using VirtuoseReality.Extension.AudioManager;

namespace TideDefense
{
    public class BeachTool : MonoBehaviour
    {
		#region Fields

		#region Tool

        protected GameplayManager _manager = null;

        [SerializeField]
        private ToolType _toolType = ToolType.None;
        public ToolType toolType
        {
            get { return _toolType; }
        }

        protected bool _isGrabbed = false;
        public ToolStatus status = ToolStatus.Dropped;

        /// <summary>
        /// Current Grid Cell the tool is dropped on
        /// </summary>
        [HideInInspector]
        public GridCellModel currentGridCell = null;

		#endregion

        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

        /// <summary>
        ///	Box Collider used to check hover and click
        /// </summary>
        [SerializeField]
        private BoxCollider _interactableBoxCollider = null;

        [SerializeField]
        private HoldClickable _holdClickable = null;

        [SerializeField]
        private Hoverable _hoverable = null;

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

        protected virtual void Start()
        {
            if (_gameplayChannel != null)
                _gameplayChannel.onChangeTool += CallbackOnChangeTool;

            InitializeHoverable();
            InitializeClickable();
        }

        protected virtual void OnDestroy()
        {
            if (_gameplayChannel != null)
                _gameplayChannel.onChangeTool -= CallbackOnChangeTool;
        }

		#endregion

		#region Tool parent class ?

        public virtual void Initialize(GameplayManager manager)
        {
            _manager = manager;
            
        }

        protected void CallbackOnChangeTool(BeachTool tool)
        {
            _isGrabbed = tool.toolType == _toolType;
            _hoverable.isInteractable = tool == null;
        }

        public virtual void SetGrabbed()
        {
            status = ToolStatus.Grabbed;
            currentGridCell = null;
            _isGrabbed = true;

            _interactableBoxCollider.enabled = false;
            _grabBoxCollider.enabled = true;
        }

        public virtual void SetDropped(GridCellModel gridCell)
        {
            status = ToolStatus.Dropped;
            currentGridCell = gridCell;
            _isGrabbed = false;

            _interactableBoxCollider.enabled = true;
            _grabBoxCollider.enabled = false;

            transform.rotation = Quaternion.identity;

            // Used by hover animation
            _currentPosition = transform.position;
            _hoveredPosition = transform.position + new Vector3(0f, _hoverYOffset, 0f);

            SoundManager.PlaySound(SoundDataIDStatic.DROP_BEACH_TOOL_SHOVEL);

            // Debug.Log(
            //     $"{toolType} has been drop on the grid : ({gridCell.coords.x}, {gridCell.coords.y} | tool {currentGridCell.currentTool})"
            // );
        }

		#endregion

		#endregion

		#region Clickable

        private void InitializeClickable()
        {
            HoldClickBehaviour holdLeftClickBehaviour = _holdClickable.GetBehaviour(
                InteractableManager.MOUSE_LEFT
            );

            holdLeftClickBehaviour.onClick += CallbackOnLeftClick;
        }

        private void CallbackOnLeftClick(RaycastHit hit)
        {
            _gameplayChannel.onClickTool.Invoke(this);
        }

		#endregion

		#region Hoverable

        public void InitializeHoverable()
        {
            if (_hoverable != null)
            {
                _hoverable.onHoverEnter += CallbackOnHoverEnter;
                _hoverable.onHoverExit += CallbackOnHoverExit;
            }
        }

        public void CallbackOnHoverEnter(RaycastHit hit)
        {
            // Bucket makes a little jump above the ground
            if (!_isGrabbed)
            {
                SoundManager.PlaySound(SoundDataIDStatic.BEACH_TOOL_HOVER_IN);
                _hoverTween.Kill();
                _hoverTween = transform
                    .DOMove(_hoveredPosition, _hoverDuration)
                    .SetEase(Ease.InCubic);
            }
        }

        public void CallbackOnHoverExit()
        {
            // Bucket goes back to position if not clicked
            if (!_isGrabbed)
            {
                SoundManager.PlaySound(SoundDataIDStatic.BEACH_TOOL_HOVER_OUT);
                _hoverTween.Kill();
                _hoverTween = transform
                    .DOMove(_currentPosition, _hoverDuration)
                    .SetEase(Ease.OutCubic);
            }
        }

		#endregion
    }
}
