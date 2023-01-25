namespace TideDefense
{
    using System;
    using PierreMizzi.TilesetUtils;
    using UnityEngine;

    public class FortificationManager : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private GridManager _gridManager = null;

        [SerializeField]
        private SandTower _prefabSandTower = null;

        [SerializeField]
        private Transform _fortificationContainer = null;

        [SerializeField] private SandCastle _sandCastle = null;

		#endregion

		#region Methods

        private void Start() {
            _sandCastle.Initialize(this);

            // Sand Castle
            _gridManager.SetSandCastleOnGrid(_sandCastle);

        }

        public void BuildSandTower(GridCellModel gridCell, float sandWaterConcentration)
        {
            Vector3 worldPosition = _gridManager.gridModel.GetCellWorldPositionFromCoordinates(
                gridCell.coords
            );

            SandTower tower = UnityEngine.Object.Instantiate(
                _prefabSandTower,
                worldPosition,
                Quaternion.identity,
                _fortificationContainer
            );

            tower.Initialize(this, gridCell, sandWaterConcentration);

            gridCell.building = tower;
        }

        public void DestroyBuilding(Building rempart)
        {
            rempart.gridCell.building = null;
            Destroy(rempart.gameObject);
        }

        public int GetRempartNeighboorsFromCoords(Vector2Int coords)
        {
            string bitmask = "";

            for (int i = 0; i < TilesetUtils.neighboorsCoordinatesFour.Count; i++)
            {
                Vector2Int offset = TilesetUtils.neighboorsCoordinatesFour[i];
                GridCellModel gridCell =
                    _gridManager.gridModel.GetCellFromCoordinates<GridCellModel>(
                        new Vector2Int(coords.x + offset.x, coords.y + offset.y)
                    );

                // Tile tile = GridManager.Instance.CurrentGrid.GetTile(x + (int)offset.x, y + (int)offset.y);

                if (gridCell == null || gridCell.building == null)
                    bitmask += 0;
                else
                    bitmask += 1;
            }

            // Debug.Log(bitmask);
            int enumValue = Convert.ToInt32(bitmask, 2);
            // Debug.Log(enumValue);

            return enumValue;
        }


		#endregion
    }
}
