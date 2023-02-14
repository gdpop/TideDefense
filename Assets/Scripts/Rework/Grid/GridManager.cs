namespace TideDefense
{
    using System;
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
        public int xLength
        {
            get { return _xLength; }
        }

        /// <summary>
        /// Amount of row from the sea the ear of the beach
        ///</summary>
        [SerializeField]
        protected int _zLength = 12;
        public int zLength
        {
            get { return _zLength; }
        }

        /// <summary>
        /// Dimension of the size of a square cell, in m
        /// </summary>
        [SerializeField]
        protected float _cellSize = 0.25f;

        public float cellSize
        {
            get { return _cellSize; }
        }

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

        #region Diggable

        [Header("Diggable")]
        [SerializeField]
        private List<DiggableHintSetting> _diggableHintSettings = new List<DiggableHintSetting>();
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
            GridCellModel cellModel = gridModel.GetCellFromCoordinates<GridCellModel>(
                visual.coords
            );
            _gameplayChannel.onLeftClickGrid.Invoke(cellModel, hit);
        }

        public void CallbackRightClick(GridCellVisual visual, RaycastHit hit)
        {
            GridCellModel cellModel = gridModel.GetCellFromCoordinates<GridCellModel>(
                visual.coords
            );
            _gameplayChannel.onRightClickGrid.Invoke(cellModel, hit);
        }

        #endregion

        #region Hoverable Callback

        public void CallbackOnHoverEnter(GridCellVisual visual, RaycastHit hit) { }

        public void CallbackOnHover(GridCellVisual visual, RaycastHit hit)
        {
            GridCellModel cellModel = gridModel.GetCellFromCoordinates<GridCellModel>(
                visual.coords
            );
            // _gameplayChannel.onHoverGrid.Invoke(cellModel, hit);
        }

        public void CallbackOnHoverExit(GridCellVisual visual) { }

        #endregion

        #endregion

        #region Diggable Hints

        public void InitializeGridView()
        {
            for (int x = 0; x < _xLength; x++)
            {
                List<GridCellVisual> column = new List<GridCellVisual>();
                for (int z = 0; z < _zLength; z++)
                {
                    GridCellVisual visual = UnityEngine.Object.Instantiate(
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

        public void DisplayDiggableHints(Dictionary<Vector2Int, Vector2Int> coordWithOffsetCoords)
        {
            GridCellModel cellModel;
            GridCellModel cellModelTool;
            GridCellVisual cellVisual;
            DiggableHintSetting setting;
            foreach (KeyValuePair<Vector2Int, Vector2Int> pair in coordWithOffsetCoords)
            {
                // We can do something if the coordinates are valid
                if (gridModel.CheckValidCoordinates(pair.Key))
                {
                    cellModel = gridModel.GetCellFromCoordinates<GridCellModel>(pair.Key);
                    cellModelTool = gridModel.GetCellFromCoordinates<GridCellModel>(
                        pair.Key + pair.Value
                    );

                    // We can do something if the cell is empty
                    if (cellModel.isEmpty)
                    {
                        setting = _diggableHintSettings.Find(
                            item => item.toolType == cellModelTool.currentTool.toolType
                        );

                        cellVisual = _gridCellVisualHash[pair.Key.x][pair.Key.y];
                        cellVisual.DisplayDiggableHints(setting);
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

        #endregion

        #region Buildable Hints

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

        public void SetSandCastleOnGrid(SandCastle sandCastle)
        {
            List<GridCellVisual> cellsAroundCastle = GetGridCellsAroundPosition(
                sandCastle.transform.position,
                0.25f
            );

            foreach (GridCellVisual cellVisual in cellsAroundCastle)
            {
                GridCellModel cellModel = gridModel.GetCellFromCoordinates<GridCellModel>(
                    cellVisual.coords
                );
                Debug.Log($"CellModel at coords {cellModel.coords} is holding the castle");
                cellModel.building = sandCastle;
            }
        }

        public List<GridCellVisual> GetGridCellsAroundPosition(Vector3 position, float radius)
        {
            List<GridCellVisual> cellsInRadius = new List<GridCellVisual>();

            for (int x = 0; x < _xLength; x++)
            {
                for (int z = 0; z < _zLength; z++)
                {
                    GridCellVisual cellVisual = _gridCellVisualHash[x][z];

                    float distance = Vector3.Magnitude(cellVisual.transform.position - position);

                    if (distance < radius)
                        cellsInRadius.Add(cellVisual);
                }
            }
            return cellsInRadius;
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
