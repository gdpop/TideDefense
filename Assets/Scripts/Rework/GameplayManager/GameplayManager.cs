namespace TideDefense
{
    using System.Collections.Generic;
    using DG.Tweening;
    using UnityEngine;

    public class GameplayManager : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private GameplayChannel _gameplayChannel = null;
        public GameplayChannel gameplayChannel
        {
            get { return _gameplayChannel; }
        }

        [SerializeField]
        private RempartsManager _rempartsManager = null;
        public RempartsManager rempartsManager
        {
            get { return _rempartsManager; }
        }

        [SerializeField]
        private SeaManager _seaManager = null;
        public SeaManager seaManager
        {
            get { return _seaManager; }
        }

        [SerializeField]
        private GridManager _gridManager = null;
        public GridManager gridManager
        {
            get { return _gridManager; }
        }

        [SerializeField]
        private Transform _gameplayContainer = null;
        public Transform gameplayContainer
        {
            get { return _gameplayContainer; }
        }

        [SerializeField]
        private Bucket _bucket = null;
        public Bucket bucket
        {
            get { return _bucket; }
        }

        [SerializeField]
        private Shovel _shovel = null;
        public Shovel shovel
        {
            get { return _shovel; }
        }

        #region State Behaviour

        private BaseGameplayBehaviour _currentStateBehaviour = null;

        private Dictionary<ToolType, BaseGameplayBehaviour> _stateBehaviours =
            new Dictionary<ToolType, BaseGameplayBehaviour>();

        #endregion

        #region Beach Tool

        /// <summary>
        /// Tool that does nothing. onChangeTool.Invoke(null); does nothing so I use it here
        /// </summary>
        private BeachTool _noneTool = new BeachTool();

        [Header("Tool")]
        [SerializeField]
        private BeachTool _currentTool = null;

        [SerializeField]
        private float _hoverBucketYOffset = 0.5f;

        private Vector3 _hoverBucketOffset = new Vector3();

        [SerializeField]
        private Rigidbody _bucketConnectedBody = null;

        [SerializeField]
        private Rigidbody _bucketJoint = null;

        [SerializeField]
        private float _grabDropTweenDuration = 0f;

        [SerializeField]
        private Transform _grabBucketAnchor = null;

        [SerializeField] private float _shovelFillingQuantity = 0.25f;
        public float shovelFillingQuantity { get { return _shovelFillingQuantity; } set { _shovelFillingQuantity = value; } }

        #endregion

		#endregion

		#region Methods

		#region MonoBehaviour

        private void Awake()
        {
            _hoverBucketOffset.y = _hoverBucketYOffset;
        }

        private void Start()
        {
            if (_gameplayChannel != null)
            {
                // _gameplayChannel.onClickBeach -= CallbackOnClickBeach;
                _gameplayChannel.onHoverBeach += CallbackOnHoverBeach;

                _gameplayChannel.onClickTool += CallbackOnClickTool;
            }

            // Initialize the bucket as dropped on the beach
            InitializeStateBehaviour();
            InitializeTool(_bucket);
            InitializeTool(_shovel);
        }

        private void OnDestroy()
        {
            if (_gameplayChannel != null)
            {
                // _gameplayChannel.onClickBeach -= CallbackOnClickBeach;
                _gameplayChannel.onHoverBeach -= CallbackOnHoverBeach;

                _gameplayChannel.onClickTool -= CallbackOnClickTool;
            }
        }

		#endregion

        #region State Behaviour

        private void InitializeStateBehaviour()
        {
            _stateBehaviours = new Dictionary<ToolType, BaseGameplayBehaviour>()
            {
                { ToolType.None, new GameplayBehaviourIdle(this) },
                { ToolType.Shovel, new GameplayBehaviourShovel(this) },
                { ToolType.Bucket, new GameplayBehaviourBucket(this) },
            };

            ChangeStateBehaviour(ToolType.None);
        }

        private void ChangeStateBehaviour(ToolType toolType)
        {
            if (_stateBehaviours[toolType] != null)
            {
                _currentStateBehaviour = _stateBehaviours[toolType];
                _currentStateBehaviour.Activate();
            }
        }

        #endregion

        private void CallbackOnHoverBeach(RaycastHit hit)
        {
            _bucketConnectedBody.MovePosition(hit.point + _hoverBucketOffset);
        }

        #region Grid Manager

        public void DropToolOnGrid(BeachTool tool, GridCell gridCell)
        {
            _gridManager.DropToolOnGrid(tool, gridCell);
        }

        public void PickToolOnGrid(BeachTool tool, GridCell gridCell)
        {
            _gridManager.PickToolOnGrid(gridCell);
        }

        #endregion

        #region Beach Tool

        private void InitializeTool(BeachTool tool)
        {
            tool.Initialize(this);
            GridCell gridCell = _gridManager.gridModel.GetCellFromWorldPosition<GridCell>(
                tool.transform.position
            );

            if (gridCell != null)
            {
                _gridManager.DropToolOnGrid(tool, gridCell);
                tool.SetDropped(gridCell);
            }
        }

        public void CallbackOnClickTool(BeachTool tool)
        {
            if (_currentTool == null)
                GrabTool(tool);
        }

        public void SetCurrentTool(BeachTool tool)
        {
            _currentStateBehaviour.Deactivate();
            _currentTool = tool;
            ChangeStateBehaviour(_currentTool.toolType);
            _gameplayChannel.onChangeTool.Invoke(_currentTool);
        }

        public void GrabTool(BeachTool tool)
        {
            // GridCell no longers hold the tool
            PickToolOnGrid(tool, tool.currentGridCell);

            SetCurrentTool(tool);

            // Tween to lerp the tool from the ground to the mouse
            Vector3 from = _currentTool.transform.position;
            LockBucketJoint();

            DOVirtual
                .Float(
                    0f,
                    1f,
                    _grabDropTweenDuration,
                    (float value) =>
                    {
                        _bucketJoint.transform.position = _bucketConnectedBody.position;
                        _currentTool.transform.position = Vector3.Lerp(
                            from,
                            _grabBucketAnchor.position,
                            value
                        );
                    }
                )
                .SetEase(Ease.OutCirc)
                .OnComplete(() =>
                {
                    // Let the tool hang free
                    FreeBucketJoint();
                    _currentTool.transform.SetParent(_bucketJoint.transform);
                    _currentTool.transform.localPosition = _grabBucketAnchor.localPosition;
                    _currentTool.SetGrabbed();
                });
        }

        private void LockBucketJoint()
        {
            _bucketJoint.isKinematic = true;
            _bucketJoint.useGravity = false;
            _bucketJoint.angularDrag = 0;
            _bucketJoint.angularVelocity = Vector3.zero;
            _bucketJoint.transform.position = _bucketConnectedBody.position;
            _bucketJoint.transform.localRotation = Quaternion.identity;
        }

        private void FreeBucketJoint()
        {
            _bucketJoint.isKinematic = false;
            _bucketJoint.useGravity = true;
            _bucketJoint.angularDrag = 0;
            _bucketJoint.angularVelocity = Vector3.zero;
            _bucketJoint.transform.position = _bucketConnectedBody.position;
            _bucketJoint.transform.localRotation = Quaternion.identity;
        }

        public void DropTool(BeachTool tool, GridCell gridCell)
        {
            LockBucketJoint();
            tool.transform.SetParent(_gameplayContainer);

            Vector3 to = _gridManager.gridModel.GetCellWorldPositionFromCoordinates(
                gridCell.coords
            );

            tool.transform
                .DOMove(to, _grabDropTweenDuration)
                .SetEase(Ease.OutCirc)
                .OnComplete(() =>
                {
                    DropToolOnGrid(tool, gridCell);
                    tool.SetDropped(gridCell);

                    SetCurrentTool(_noneTool);
                });
        }

        #endregion

    	#endregion
    }
}
