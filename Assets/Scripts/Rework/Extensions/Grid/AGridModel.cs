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

        // [SerializeField]
        // private Transform _beachTransform = null;

        // private Vector2 _flattenBeachPosition = new Vector2();

		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour

        public virtual void Initialize<T>(int xLength = 10, int zLength = 10, float cellSize = 0.25f) where T : AGridCell, new()
        {
            // _flattenBeachPosition = new Vector2(
            //     _beachTransform.position.x,
            //     _beachTransform.position.z
            // );

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
            for (int x = 0; x < _xLength; x++)
            {
                List<T> zColumn = new List<T>();
                for (int z = 0; z < _zLength; z++)
                {

                    T gridCell = new T();
                    gridCell.coords = new Vector2(x, z);
                    zColumn.Add(gridCell);
                }
                _gridCellHash.Add(zColumn as List<AGridCell>);
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





		#endregion
    }
}
