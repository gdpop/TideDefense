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

        #region Bucket

        [SerializeField]
        private ToolType _currentTool = ToolType.None;

        [SerializeField]
        private Bucket _bucket = null;

        [SerializeField]
        private float _hoverBucketYOffset = 0.5f;

        private Vector3 _hoverBucketOffset = new Vector3();

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
                _gameplayChannel.onClickBucket += CallbackOnClickBucket;
                _gameplayChannel.onHoverBeach += CallbackOnHoverBeach;
            }
        }

        private void OnDestroy()
        {
            if (_gameplayChannel != null)
            {
                _gameplayChannel.onClickGrid -= CallbackOnClickGrid;
                _gameplayChannel.onClickBucket -= CallbackOnClickBucket;
                _gameplayChannel.onHoverBeach -= CallbackOnHoverBeach;
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
            // Vector3 gridWorldPosition = _gridManager.GetCellWorldPositionFromWorldPosition(hit.point);
            if (_currentTool == ToolType.Bucket)
            {
                MoveBucket(hit.point);
            }
        }

		#endregion

        #region Bucket

        public void CallbackOnClickBucket()
        {
            if (_currentTool == ToolType.None)
            {
                GrabBucket();
            }
        }

        private void GrabBucket()
        {
            _currentTool = ToolType.Bucket;
            _bucket.transform.position = _bucket.transform.position + _hoverBucketOffset;

        }

        private void MoveBucket(Vector3 hoveredPosition)
        {
            _bucket.transform.position = hoveredPosition + _hoverBucketOffset;
        }

        private void DropBucket(Vector2Int coords)
        {
            _currentTool = ToolType.None;
            _bucket.transform.position = _gridManager.GetCellWorldPositionFromCoordinates(coords);
        }

        #endregion
    }
}
