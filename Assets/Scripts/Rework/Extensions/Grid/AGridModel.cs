namespace PierreMizzi.Grid
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;

    [ExecuteInEditMode]
    public abstract class AGridModel
    {
		#region Fields

		#region Grid

        /// <summary>
        ///	Ammount of columns along the side of the beach
        ///</summary>
        protected int _xLength = 10;

        /// <summary>
        /// Amount of row from the sea the ear of the beach
        ///</summary>
        protected int _zLength = 10;

        /// <summary>
        /// Dimension of the size of a square cell, in m
        /// </summary>
        protected float _cellSize = 0.25f;

        /// <summary>
        ///	Hash of the cells created. _gridCellHash(3)(2) will return the Cell at coordinates (x = 3; z = 2)
        /// </summary>
        protected List<List<AGridCell>> _gridCellHash = new List<List<AGridCell>>();

        private static Vector2Int unvalidGridCoords = new Vector2Int(-1, -1);

		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour

        public virtual void Initialize<T>(int xLength, int zLength, float cellSize)
            where T : AGridCell, new()
        {
            _xLength = xLength;
            _zLength = zLength;
            _cellSize = cellSize;

            CreateLogicalGrid<T>();
        }

		#endregion

		#region Grid

        /// <summary>
        ///	Fills the double entry array _gridCellHash with freshly created GridCell
        /// </summary>
        public void CreateLogicalGrid<T>() where T : AGridCell, new()
        {
            _gridCellHash = new List<List<AGridCell>>();

            for (int x = 0; x < _xLength; x++)
            {
                List<AGridCell> zColumn = new List<AGridCell>();
                for (int z = 0; z < _zLength; z++)
                {
                    T gridCell = new T();
                    gridCell.coords = new Vector2Int(x, z);
                    zColumn.Add(gridCell);
                }
                _gridCellHash.Add(zColumn);
            }
        }

        /// <summary>
        ///	Given a WorldPosition, it returns the coordinates of the corresponding grid cell
        /// </summary>
        public virtual Vector2Int GetCellCoordinatesFromWorldPosition(Vector3 worldPosition)
        {
            Vector2 flattenPos = new Vector2(worldPosition.x, worldPosition.z);
            Vector2Int coords = new Vector2Int(
                (int)Mathf.Floor(flattenPos.x / _cellSize),
                (int)Mathf.Floor(flattenPos.y / _cellSize)
            );

            if (coords.x < 0 || coords.x > _xLength - 1 || coords.y < 0 || coords.y > _zLength - 1)
            {
                Debug.Log($"Given position is out of grid : ({coords.x};{coords.y})");
                return unvalidGridCoords;
            }

            return coords;
        }

        public virtual T GetCellFromCoordinates<T>(Vector2Int coords) where T : AGridCell
        {
            if (CheckValidCoordinates(coords))
            {
                return _gridCellHash[coords.x][coords.y] as T;
            }
            else
                return null;
        }

        public virtual T GetCellFromWorldPosition<T>(Vector3 worldPosition) where T : AGridCell
        {
            Vector2Int coords = GetCellCoordinatesFromWorldPosition(worldPosition);

            if (coords != unvalidGridCoords)
            {
                return GetCellFromCoordinates<T>(coords);
            }
            else
                return null;
        }

        public virtual bool CheckValidCoordinates(Vector2Int coords)
        {
            if (coords.x < 0 || _xLength - 1 < coords.x)
            {
                // Debug.LogWarning($"{coords.x}:{coords.y} : {coords.x} is wrong");
                return false;
            }

            if (coords.y < 0 || _zLength - 1 < coords.y)
            {
                // Debug.LogWarning($"{coords.x}:{coords.y} : {coords.y} is wrong");
                return false;
            }

            return true;
        }

        public virtual Vector3 GetPositionFromCoordinates(Vector2Int coords)
        {
            if (CheckValidCoordinates(coords))
                return new Vector3(
                    (coords.x * _cellSize) + _cellSize / 2f,
                    0f,
                    (coords.y * _cellSize) + _cellSize / 2f
                );
            else
                return Vector3.zero;
        }

		#endregion





		#endregion
    }
}
