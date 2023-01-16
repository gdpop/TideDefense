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

        [SerializeField] private Transform _bucketJoint = null;

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

        private void CallbackOnClickGrid(Vector2Int coords)
        {
            if (_currentTool == ToolType.None)
                _rempartsManager.BuildRempart(coords);
            else if (_currentTool == ToolType.Bucket)
                DropBucket(coords);
        }

        private void CallbackOnHoverBeach(RaycastHit hit)
        {
            if (_currentTool == ToolType.Bucket)
                MoveBucket(hit.point);
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
            _currentTool = ToolType.Bucket;
            _gameplayChannel.onChangeTool.Invoke(_currentTool);

            // Manage ConnectedBody
            _bucketConnectedBody.position = _bucket.transform.position + _hoverBucketOffset;

            // Manage Bucket
            _bucket.SetGrabbed();
            _bucket.transform.SetParent(_bucketJoint);
            _bucket.transform.localPosition = new Vector3(0f, -0.5f, 0f);

        }

        private void MoveBucket(Vector3 hoveredPosition)
        {
            _bucketConnectedBody.transform.position = hoveredPosition + _hoverBucketOffset;
        }

        private void DropBucket(Vector2Int coords)
        {
            _currentTool = ToolType.None;
            _gameplayChannel.onChangeTool.Invoke(_currentTool);

            _bucket.transform.SetParent(_gameplayContainer);
            _bucket.transform.position = _gridManager.GetCellWorldPositionFromCoordinates(coords);
            _bucket.SetDropped();
        }

        #endregion
    }
}
