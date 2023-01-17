namespace TideDefense
{
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

        private void Start()
        {
            gridModel = new GridModel();
            gridModel.Initialize<GridCell>(_xLength, _zLength, _cellSize, _yElevation, _beachSlope);

            if (_gameplayChannel != null)
                _gameplayChannel.onClickBeach += CallbackOnClickBeach;
        }

        private void OnDestroy()
        {
            if (_gameplayChannel != null)
                _gameplayChannel.onClickBeach -= CallbackOnClickBeach;
        }

        protected virtual void OnDrawGizmos()
        {
            if (_displayGizmos)
                DrawGridGizmos();
        }

		#region Gameplay

        private void CallbackOnClickBeach(RaycastHit hit)
        {
            GridCell gridCell = gridModel.GetCellFromWorldPosition<GridCell>(hit.point);

            if (gridCell != null)
                _gameplayChannel.onClickGrid.Invoke(gridCell, hit);
        }

        public void DropToolOnGrid(ToolType toolType, GridCell gridCell)
        {
            gridCell.currentTool = toolType;
        }

        public void PickToolOnGrid(GridCell gridCell)
        {
            gridCell.currentTool = ToolType.None;
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
