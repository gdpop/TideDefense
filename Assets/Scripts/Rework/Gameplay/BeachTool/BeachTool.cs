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
        private BeachToolType _toolType = BeachToolType.None;
        public BeachToolType toolType
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

		#region Hoverable

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

        #region Washed Up
            
        private bool _isWashedUp = false;
        public bool isWashedUp { get { return _isWashedUp; } set {_isWashedUp = value;}}

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

		#region Behaviour

        public virtual void Initialize(GameplayManager manager)
        {
            _manager = manager;
        }

        protected void CallbackOnChangeTool(BeachTool tool)
        {
            if(tool.toolType == _toolType)
            {
                _isGrabbed = true;
                SetNonInteractable();
            }
            else
            {
                _isGrabbed = false;
                SetInteractable();
            }
        }

        public virtual void SetGrabbed()
        {
            status = ToolStatus.Grabbed;
            currentGridCell = null;
            _isGrabbed = true;

            SetNonInteractable();
        }

        public virtual void SetDropped(GridCellModel gridCell)
        {
            status = ToolStatus.Dropped;
            currentGridCell = gridCell;
            _isGrabbed = false;

            SetInteractable();

            // Used by hover animation
            _currentPosition = transform.position;
            _hoveredPosition = transform.position + new Vector3(0f, _hoverYOffset, 0f);

            SoundManager.PlaySound(SoundDataIDStatic.DROP_BEACH_TOOL_SHOVEL);
        }

        private void SetInteractable()
        {
            _holdClickable.isInteractable = true;
            _hoverable.isInteractable = true;

            // Needed so it can hang from the BeahToolHolder
            _interactableBoxCollider.enabled = true;
            _grabBoxCollider.enabled = false;
        }

        private void SetNonInteractable()
        {
            _holdClickable.isInteractable = false;
            _hoverable.isInteractable = false;

            // Needed so it can hang from the BeahToolHolder
            _interactableBoxCollider.enabled = false;
            _grabBoxCollider.enabled = true;
        }

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

        #region Sequencer

        public virtual void StartFloating()
        {
            SetNonInteractable();
        }

        public virtual void WashUpComplete()
        {
            // General Status
            _isWashedUp = true;
            status = ToolStatus.Dropped;
            _isGrabbed = false;


            // Hover animation
            _currentPosition = transform.position;
            _hoveredPosition = transform.position + new Vector3(0f, _hoverYOffset, 0f);


            // Interactions
            SetInteractable();

            if (_gameplayChannel != null)
                _gameplayChannel.onChangeTool += CallbackOnChangeTool;

            InitializeHoverable();
            InitializeClickable();
        }

            
        #endregion

		#endregion
    }
}
