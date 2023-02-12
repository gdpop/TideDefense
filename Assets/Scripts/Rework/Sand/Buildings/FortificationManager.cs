namespace TideDefense
{
    using System;
    using PierreMizzi.TilesetUtils;
    using UnityEngine;
    using ToolBox.Pools;
    using System.Collections.Generic;
    using VirtuoseReality.Extension.AudioManager;

    public class FortificationManager : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private GridManager _gridManager = null;
      
        [SerializeField]
        private Transform _fortificationContainer = null;

        private List<Building> _buildings = new List<Building>();

        [SerializeField]
        private SandCastle _sandCastle = null;

		#endregion

		#region Methods

        #region MonoBehaviour

        private void Start()
        {
            _sandCastle.Initialize(this);

            // Sand Castle
            _gridManager.SetSandCastleOnGrid(_sandCastle);
        }

        #endregion

        public void CastMould(MouldTool tool, GridCellModel gridCell, float sandWaterConcentration)
        {
            Vector3 worldPosition = _gridManager.gridModel.GetCellWorldPositionFromCoordinates(
                gridCell.coords
            );

            Fortification fortification = UnityEngine.Object.Instantiate(
                tool.mouldedShape.shape,
                worldPosition,
                Quaternion.identity,
                _fortificationContainer
            );

            fortification.Initialize(this, gridCell, sandWaterConcentration);
            _buildings.Add(fortification);

            RefreshLinkingRemparts();

            SoundManager.PlaySound(SoundDataIDStatic.CREATE_FORTIFICATION);
        }

        public void DestroyBuilding(Building building)
        {
            building.gridCellModel.building = null;

            if (_buildings.Contains(building))
                _buildings.Remove(building);

            Destroy(building.gameObject);
        }

        private void RefreshLinkingRemparts()
        {
            SandTower sandTower;
            Vector2Int direction;
            Vector2Int coords;
            GridCellModel cellModel;
            List<int> linkingRempartsIndex = new List<int>();

            foreach (Building building in _buildings)
            {
                if (building.TryGetComponent(out sandTower))
                {
                    linkingRempartsIndex.Clear();
                    // We check every neighboords to see if their is a fortification in place
                    for (int i = 0; i < TilesetUtils.neighboorsCoordinatesFour.Count; i++)
                    {
                        direction = TilesetUtils.neighboorsCoordinatesFour[i];
                        coords =
                            sandTower.gridCellModel.coords
                            + direction;

                        if (!_gridManager.gridModel.CheckValidCoordinates(coords))
                            continue;

                        cellModel = _gridManager.gridModel.GetCellFromCoordinates<GridCellModel>(
                            coords
                        );

                        //If there is one, we add the index of the direction into linkingRempartsIndex
                        if (cellModel.building != null && CheckLink(direction, cellModel.building.tilesetType))
                            linkingRempartsIndex.Add(i);
                    }
                    // We tellt he send tower which linkingRemparts to display
                    sandTower.RefreshLinkingRemparts(linkingRempartsIndex);
                }
            }
        }

        [Obsolete]
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

        public bool CheckLink(Vector2Int direction, TilesetTypeFour tilesetType)
        {
            if(TilesetUtils.directionToLinkableTilesetTypes.ContainsKey(direction))
                return TilesetUtils.directionToLinkableTilesetTypes[direction].Contains(tilesetType);
            else
                return false;
        }


		#endregion
    }
}
