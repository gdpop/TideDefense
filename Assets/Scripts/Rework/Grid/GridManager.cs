namespace TideDefense
{
    using UnityEngine;
    using System.Collections.Generic;

    [ExecuteInEditMode]
    public class GridManager : MonoBehaviour
    {
		#region Fields

		#region Grid

        /// <summary>
        ///	Ammount of columns along the side of the beach
        ///</summary>
        [SerializeField]
        private int _xLength = 10;

        /// <summary>
        /// Amount of row from the sea the ear of the beach
        ///</summary>
        [SerializeField]
        private int _zLength = 10;

        /// <summary>
        /// Dimension of the size of a square cell, in m
        /// </summary>
        [SerializeField]
        private float _cellSize = 0.25f;

        /// <summary>
        ///	Hash of the cells created. _gridCellHash(3)(2) will return the Cell at coordinates (x = 3; z = 2)
        /// </summary>
        private List<List<GridCell>> _gridCellHash = new List<List<GridCell>>();

        [SerializeField]
        private Transform _beachTransform = null;

        private Vector2 _flattenBeachPosition = new Vector2();

		#endregion

		#region Grid Gizmos

        [Header("Gizmos")]
        [SerializeField]
        private bool _displayGizmos = true;

        [SerializeField]
        private Color _gizmoColor = new Color();

        [SerializeField]
        private float _beachSlope = 5f;

        private float _yElevation = 0f;

		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour

        private void OnDrawGizmos()
        {
            if (_displayGizmos)
                DrawGridGizmos();
        }

        private void Start()
        {
            _flattenBeachPosition = new Vector2(
                _beachTransform.position.x,
                _beachTransform.position.z
            );

            CreateLogicalGrid();
        }

		#endregion

		#region Methods

        /// <summary>
        ///	Fills the double entry array _gridCellHash with freshly created GridCell
        /// </summary>
        public void CreateLogicalGrid()
        {
            for (int x = 0; x < _xLength; x++)
            {
                List<GridCell> zColumn = new List<GridCell>();
                for (int z = 0; z < _zLength; z++)
                {
                    GridCell gridCell = new GridCell();
                    zColumn.Add(gridCell);
                }
                _gridCellHash.Add(zColumn);
            }
        }

        /// <summary>
        ///	Given a WorldPosition, it returns the coordinates of the corresponding grid cell
        /// </summary>
        public Vector2 WorldPositionToCellCoordinates(Vector3 worldPosition)
        {
            Vector2 flattenPos = new Vector2(worldPosition.x, worldPosition.z);
            Vector2 coordinates = new Vector2(
                Mathf.Floor(flattenPos.x / _cellSize),
                Mathf.Floor(flattenPos.y / _cellSize)
            );

            if (
                coordinates.x < 0
                || coordinates.x > _xLength - 1
                || coordinates.y < 0
                || coordinates.y > _zLength - 1
            )
            {
                Debug.Log($"Given position is out of grid : ({coordinates.x};{coordinates.y})");
                return Vector2.zero;
            }

            return coordinates;
        }

		#endregion

		#region Grid Gizmos


        private void DrawGridGizmos()
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


        [SerializeField]
        private Transform _debugTest = null;

        [ContextMenu("Debug_Grid")]
        public void Debug_Grid()
        {
            Debug.Log(WorldPositionToCellCoordinates(_debugTest.position));
        }

		#endregion
    }
}
