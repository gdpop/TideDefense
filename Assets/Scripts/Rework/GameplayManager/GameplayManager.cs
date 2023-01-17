namespace TideDefense
{
    using UnityEngine;

    public class GameplayManager : MonoBehaviour
    {
		#region Fields


        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

        [SerializeField]
        private RempartsManager _rempartsManager = null;

        [SerializeField]
        private GridManager _gridManager = null;

        [SerializeField] private Transform _gameplayContainer = null;

        #region Bucket

        [Header("Bucket")]
        [SerializeField]
        private ToolType _currentTool = ToolType.None;

        [SerializeField]
        private Bucket _bucket = null;

        [SerializeField]
        private float _hoverBucketYOffset = 0.5f;

        private Vector3 _hoverBucketOffset = new Vector3();

        [SerializeField] private Rigidbody _bucketConnectedBody = null;

        [SerializeField] private Rigidbody _bucketJoint = null;

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
                _gameplayChannel.onClickGrid += CallbackOnClickGrid;
                _gameplayChannel.onHoverBeach += CallbackOnHoverBeach;

                _gameplayChannel.onClickBucket += CallbackOnClickBucket;
            }

            // Initialize the bucket as dropped on the beach
            InitializeBucket();
            
        }

        private void OnDestroy()
        {
            if (_gameplayChannel != null)
            {
                _gameplayChannel.onClickGrid -= CallbackOnClickGrid;
                _gameplayChannel.onHoverBeach -= CallbackOnHoverBeach;

                _gameplayChannel.onClickBucket -= CallbackOnClickBucket;
            }
        }

		#endregion

        private void CallbackOnClickGrid(GridCell gridCell, RaycastHit hit)
        {
            if (_currentTool == ToolType.None)
                _rempartsManager.BuildRempart(gridCell);
            else if (_currentTool == ToolType.Bucket)
                DropBucket(gridCell);
        }

        private void CallbackOnHoverBeach(RaycastHit hit)
        {
            // _bucketConnectedBody.transform.position = hit.point + _hoverBucketOffset;
            _bucketConnectedBody.MovePosition(hit.point + _hoverBucketOffset);
        }

		#endregion

        #region Tools

        public void DropToolOnGrid(ToolType toolType, GridCell gridCell)
        {
            _gridManager.DropToolOnGrid(toolType, gridCell);
        }

        public void PickToolOnGrid(ToolType toolType, GridCell gridCell)
        {
            _gridManager.PickToolOnGrid(gridCell);
        }
            
        #endregion

        #region Bucket

        public void CallbackOnClickBucket()
        {
            if (_currentTool == ToolType.None)
                GrabBucket();
        }

        private void GrabBucket()
        {
            PickToolOnGrid(ToolType.Bucket, _bucket.currentGridCell);
            _currentTool = ToolType.Bucket;
            _gameplayChannel.onChangeTool.Invoke(_currentTool);

            // Manage ConnectedBody
            _bucketConnectedBody.position = _bucket.transform.position + _hoverBucketOffset;

            // Manage Bucket
            _bucket.SetGrabbed();
            _bucket.transform.SetParent(_bucketJoint.transform);
            _bucket.transform.localPosition = new Vector3(0f, -0.25f, 0f);

        }

        private void InitializeBucket()
        {
            GridCell gridCell = _gridManager.gridModel.GetCellFromWorldPosition<GridCell>(_bucket.transform.position);

            if(gridCell != null)
            {
                _gridManager.DropToolOnGrid(ToolType.Bucket, gridCell);
                _bucket.SetDropped(gridCell);
            }
        }

        private void DropBucket(GridCell gridCell)
        {
            DropToolOnGrid(ToolType.Bucket, gridCell);
            _currentTool = ToolType.None;
            _gameplayChannel.onChangeTool.Invoke(_currentTool);



            _bucket.transform.SetParent(_gameplayContainer);
            _bucket.transform.position = _gridManager.gridModel.GetCellWorldPositionFromCoordinates(gridCell.coords);
            _bucket.SetDropped(gridCell);

            _bucketJoint.angularDrag = 0f;
            _bucketJoint.angularVelocity = Vector3.zero;
            _bucketJoint.transform.localRotation = Quaternion.identity;
        }

        #endregion
    }
}
