namespace TideDefense
{
    using System.Collections.Generic;
    using PierreMizzi.TilesetUtils;
    using UnityEngine;

    public class GridManager : MonoBehaviour
    {
		#region Fields

		#region Gameplay

        [HideInInspector]
        public GridModel gridModel = null;

        /// <summary>
        ///	Ammount of columns along the side of the beach
        ///</summary>
        [SerializeField]
        protected int _xLength = 12;

        /// <summary>
        /// Amount of row from the sea the ear of the beach
        ///</summary>
        [SerializeField]
        protected int _zLength = 12;

        /// <summary>
        /// Dimension of the size of a square cell, in m
        /// </summary>
        [SerializeField]
        protected float _cellSize = 0.25f;

        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

        #endregion

        #region Grid View

        [Header("Grid View")]
        [SerializeField]
        private Transform _gridCellVisualContainer = null;

        [SerializeField]
        private GridCellVisual _gridCellVisualPrefab = null;

        [SerializeField]
        private List<List<GridCellVisual>> _gridCellVisualHash = new List<List<GridCellVisual>>();

        #endregion

		#region Grid Gizmos

        [Header("Gizmos")]
        [SerializeField]
        protected bool _displayGizmos = true;

        [SerializeField]
        protected Color _gizmoColor = new Color();

        [SerializeField]
        protected float _beachSlope = 5f;

        protected float _yElevation = 0f;

		#endregion

		#endregion

		#region Methods

        #region MonoBehaviour


        private void Start()
        {
            gridModel = new GridModel();
            gridModel.Initialize<GridCellModel>(
                _xLength,
                _zLength,
                _cellSize,
                _yElevation,
                _beachSlope
            );

            InitializeGridView();
        }

        protected virtual void OnDrawGizmos()
        {
            if (_displayGizmos)
                DrawGridGizmos();
        }

        #endregion

		#region Gameplay

        private void CallbackOnClickBeach(RaycastHit hit)
        {
            GridCellModel gridCell = gridModel.GetCellFromWorldPosition<GridCellModel>(hit.point);

            if (gridCell != null)
                _gameplayChannel.onClickGrid.Invoke(gridCell, hit);
        }

        public void DropToolOnGrid(BeachTool tool, GridCellModel gridCell)
        {
            gridCell.currentTool = tool;
        }

        public void PickToolOnGrid(GridCellModel gridCell)
        {
            gridCell.currentTool = null;
        }

        #region Clickable Callback

        public void CallbackLeftClick(GridCellVisual visual, RaycastHit hit)
        {
            Debug.Log("Clicked !" +visual.coords);
            GridCellModel cellModel = gridModel.GetCellFromCoordinates<GridCellModel>(
                visual.coords
            );
            _gameplayChannel.onClickGrid.Invoke(cellModel, hit);
        }

        #endregion

        #region HoldClickable Callback

        public void CallbackStartHoldClickLeft(GridCellVisual visual)
        {
            if (_gameplayChannel != null)
                _gameplayChannel.OnStartHoldClickGrid.Invoke();
        }

        public void CallbackProgressHoldClickLeft(GridCellVisual visual, float progress)
        {
            if (_gameplayChannel != null)
                _gameplayChannel.OnProgressHoldClickGrid.Invoke(progress);
        }

        public void CallbackCompleteHoldClickLeft(GridCellVisual visual)
        {
            Debug.Log("Completed !" +visual.coords);
            GridCellModel cellModel = gridModel.GetCellFromCoordinates<GridCellModel>(
                visual.coords
            );

            if (_gameplayChannel != null)
                _gameplayChannel.OnCompleteHoldClickGrid.Invoke(cellModel);
        }

        public void CallbackCancelHoldClickLeft(GridCellVisual visual)
        {
            if (_gameplayChannel != null)
                _gameplayChannel.OnCancelHoldClickGrid.Invoke();
        }

        #endregion

        #region Hoverable Callback

        public void CallbackOnHoverEnter(GridCellVisual visual, RaycastHit hit) { }

        public void CallbackOnHover(GridCellVisual visual, RaycastHit hit)
        {
            GridCellModel cellModel = gridModel.GetCellFromCoordinates<GridCellModel>(
                visual.coords
            );
            _gameplayChannel.onHoverGrid.Invoke(cellModel, hit);
        }

        public void CallbackOnHoverExit(GridCellVisual visual) { }

        #endregion

        #endregion

        #region Grid View

        public void InitializeGridView()
        {
            for (int x = 0; x < _xLength; x++)
            {
                List<GridCellVisual> column = new List<GridCellVisual>();
                for (int z = 0; z < _zLength; z++)
                {
                    GridCellVisual visual = Object.Instantiate(
                        _gridCellVisualPrefab,
                        _gridCellVisualContainer
                    );
                    visual.Initialize(this, new Vector2Int(x, z));

                    Vector3 worldPosition =
                        gridModel.GetCellWorldPositionFromCoordinates(new Vector2Int(x, z))
                        + new Vector3(0f, 0.01f, 0f);
                    visual.transform.position = worldPosition;
                    visual.name = $"GridCellVisual_{x}_{z}";

                    column.Add(visual);
                }
                _gridCellVisualHash.Add(column);
            }
        }

        public void DisplayDiggableHints()
        {
            Vector2Int coords = new Vector2Int();

            // We got throught the hash of GridCell of the model to find the GridCell containing the bucket
            for (int x = 0; x < _xLength; x++)
            {
                for (int z = 0; z < _zLength; z++)
                {
                    coords = new Vector2Int(x, z);
                    GridCellModel cellModel = gridModel.GetCellFromCoordinates<GridCellModel>(
                        coords
                    );

                    // We found the GridCell containing the bucket !
                    if (
                        cellModel.currentTool != null
                        && cellModel.currentTool.toolType == ToolType.Bucket
                    )
                    {
                        Vector2Int neighboorCoords = new Vector2Int();
                        GridCellModel neighboorCellModel = null;
                        GridCellVisual cellVisual = null;

                        // We go through all 8 surrouding cells
                        for (int i = 0; i < TilesetUtils.neighboorsCoordinatesEight.Count; i++)
                        {
                            neighboorCoords =
                                cellModel.coords + TilesetUtils.neighboorsCoordinatesEight[i];
                            // Check if the coords are not out of the grid AND check if the gridCell is empty
                            if (gridModel.CheckValidCoordinates(neighboorCoords))
                            {
                                neighboorCellModel =
                                    gridModel.GetCellFromCoordinates<GridCellModel>(
                                        neighboorCoords
                                    );

                                // We check the cell is actually empty
                                if (neighboorCellModel.isEmpty)
                                {
                                    cellVisual = _gridCellVisualHash[neighboorCoords.x][
                                        neighboorCoords.y
                                    ];
                                    cellVisual.DisplayDiggableHints();
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }

        public void HideDiggableHints()
        {
            GridCellVisual cellVisual = null;

            for (int x = 0; x < _xLength; x++)
            {
                for (int z = 0; z < _zLength; z++)
                {
                    cellVisual = _gridCellVisualHash[x][z];
                    cellVisual.HideDiggableHints();
                }
            }
        }

        public void DisplayBuildableHints()
        {
            Vector2Int coords = new Vector2Int();
            GridCellModel cellModel = null;
            GridCellVisual cellVisual = null;

            for (int x = 0; x < _xLength; x++)
            {
                for (int z = 0; z < _zLength; z++)
                {
                    coords = new Vector2Int(x, z);

                    cellModel = gridModel.GetCellFromCoordinates<GridCellModel>(coords);
                    cellVisual = _gridCellVisualHash[x][z];

                    if (cellModel.isEmpty)
                        cellVisual.DisplayBuildableHints();
                }
            }
        }

        public void HideBuildableHints()
        {
            GridCellVisual cellVisual = null;

            for (int x = 0; x < _xLength; x++)
            {
                for (int z = 0; z < _zLength; z++)
                {
                    cellVisual = _gridCellVisualHash[x][z];
                    cellVisual.HideBuildableHints();
                }
            }
        }

        #endregion

		#region Grid Gizmos

        protected void DrawGridGizmos()
        {
            _yElevation = _cellSize * Mathf.Tan(Mathf.Deg2Rad * _beachSlope);

            Gizmos.color = _gizmoColor;

            Vector3 from;
            Vector3 to;

            for (int x = 0; x < _xLength + 1; x++)
            {
                from = new Vector3(x * _cellSize, 0f, 0f);
                to = new Vector3(from.x, (_zLength - 1) * _yElevation, _zLength * _cellSize);

                Gizmos.DrawLine(from, to);
                for (int z = 0; z < _zLength + 1; z++)
                {
                    from = new Vector3(0, z * _yElevation, z * _cellSize);
                    to = new Vector3(_xLength * _cellSize, from.y, from.z);

                    Gizmos.DrawLine(from, to);
                }
            }
        }

		#endregion

		#endregion
    }
}
