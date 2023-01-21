namespace TideDefense
{
    using PierreMizzi.Grid;
    using UnityEngine;

    public class GridModel : AGridModel
    {
        public float _yElevation = 0f;
        public float _beachSlope = 0f;

        public virtual void Initialize<T>(
            int xLength,
            int zLength,
            float cellSize,
            float yElevation,
            float beachSlope
        ) where T : AGridCell, new()
        {
            _yElevation = yElevation;
            _beachSlope = beachSlope;

            Initialize<GridCellModel>(xLength, zLength, cellSize);
        }

        public Vector3 GetCellWorldPositionFromWorldPosition(Vector3 worldPosition)
        {
            Vector2Int clickedGridCoords = GetCellCoordinatesFromWorldPosition(worldPosition);

            return GetCellWorldPositionFromCoordinates(clickedGridCoords);
        }

        public Vector3 GetCellWorldPositionFromCoordinates(Vector2Int coords)
        {
            if (!CheckValidCoordinates(coords))
            {
                Debug.LogError($"coords out of bound : {coords}");
                return Vector3.zero;
            }

            Vector3 gridWorldPosition = GetPositionFromCoordinates(coords);

            _yElevation =
                (coords.y * _cellSize + (_cellSize / 2f)) * Mathf.Tan(Mathf.Deg2Rad * _beachSlope);

            return new Vector3(gridWorldPosition.x, _yElevation - 0.015f, gridWorldPosition.z);
        }

    }
}
