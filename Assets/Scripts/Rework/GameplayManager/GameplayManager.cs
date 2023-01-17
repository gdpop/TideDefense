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

        [SerializeField]
        private RempartsManager _rempartsManager = null;

        [SerializeField]
        private GridManager _gridManager = null;

        [SerializeField] private Transform _gameplayContainer = null;

        #region Tool

        [Header("Tool")]
        [SerializeField]
        private BeachTool _currentTool = null;

        [SerializeField] 
        private List<BeachTool> _tools = new List<BeachTool>();

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

                _gameplayChannel.onClickTool += CallbackOnClickTool;
            }

            // Initialize the bucket as dropped on the beach
            InitializeTool();
            
        }

        private void OnDestroy()
        {
            if (_gameplayChannel != null)
            {
                _gameplayChannel.onClickGrid -= CallbackOnClickGrid;
                // _gameplayChannel.onClickBeach -= CallbackOnClickBeach;
                _gameplayChannel.onHoverBeach -= CallbackOnHoverBeach;

                _gameplayChannel.onClickTool -= CallbackOnClickTool;
            }
        }

		#endregion

        private void CallbackOnClickGrid(GridCell gridCell, RaycastHit hit)
        {
            if (_currentTool == null)
                _rempartsManager.BuildRempart(gridCell);
            else
                DropTool(_currentTool, gridCell);
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

        public void CallbackOnClickTool(BeachTool tool)
        {
            if (_currentTool == null)
                GrabTool(tool);
        }

        private void GrabTool(BeachTool tool)
        {
            PickToolOnGrid(tool.toolType, tool.currentGridCell);
            _currentTool = tool;
            _gameplayChannel.onChangeTool.Invoke(_currentTool);

            // Manage ConnectedBody
            // _bucketConnectedBody.position = _bucket.transform.position + _hoverBucketOffset;

            // Manage Bucket
            // _bucket.SetGrabbed();
            // _bucket.transform.SetParent(_bucketJoint.transform);
            // _bucket.transform.localPosition = new Vector3(0f, -0.25f, 0f);

            // Reworked
            Vector3 from = _currentTool.transform.position;
            LockBucketJoint();

            DOVirtual.Float(0f, 1f, _grabDropTweenDuration,(float value)=>{
                
                _bucketJoint.transform.position = _bucketConnectedBody.position;
                _currentTool.transform.position = Vector3.Lerp(from, _grabBucketAnchor.position, value);
                
            }).SetEase(Ease.OutCirc).OnComplete(()=>{

                FreeBucketJoint();
                _currentTool.transform.SetParent(_bucketJoint.transform);
                _currentTool.transform.localPosition = _grabBucketAnchor.localPosition;
                _currentTool.SetGrabbed();
            });
        }

        [SerializeField] private float _grabDropTweenDuration = 0f;

        [SerializeField] private Transform _grabBucketAnchor = null;

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


        private void InitializeTool()
        {
            foreach (BeachTool tool in _tools)
            {
                GridCell gridCell = _gridManager.gridModel.GetCellFromWorldPosition<GridCell>(tool.transform.position);

                if(gridCell != null)
                {
                    _gridManager.DropToolOnGrid(tool.toolType, gridCell);
                    tool.SetDropped(gridCell);
                }
            }
        }

        private void DropTool(BeachTool tool, GridCell gridCell)
        {
            LockBucketJoint();
            tool.transform.SetParent(_gameplayContainer);

            Vector3 to = _gridManager.gridModel.GetCellWorldPositionFromCoordinates(gridCell.coords);

            tool.transform.DOMove(to, _grabDropTweenDuration).SetEase(Ease.OutCirc).OnComplete(()=>{
                DropToolOnGrid(tool.toolType, gridCell);
                tool.SetDropped(gridCell);

                _currentTool = null;
                _gameplayChannel.onChangeTool.Invoke(null);
            });
        }

        #endregion
    }
}
