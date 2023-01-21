namespace TideDefense
{
    using System;
    using System.Collections.Generic;
    using PierreMizzi.TilesetUtils;
    using UnityEngine;

    public class RempartsManager : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private GridManager _gridManager = null;

        [SerializeField]
        private List<RempartBlock> _rempartBlocks = new List<RempartBlock>();

        [SerializeField]
        private Rempart _prefabRempart = null;

        [SerializeField]
        private Transform _rempartContainer = null;

        [SerializeField]
        private Rempart _simpleRempart;

		#endregion

		#region Methods

        public void BuildRempart(GridCellModel gridCell)
        {
            if (gridCell == null || !gridCell.isEmpty)
                return;

            RempartBlock rempartBlock = GetRempartBlockFromCoords(gridCell.coords);
            Vector3 worldPosition = _gridManager.gridModel.GetCellWorldPositionFromCoordinates(
                gridCell.coords
            );

            Rempart rempart = UnityEngine.Object.Instantiate(
                _prefabRempart,
                worldPosition,
                Quaternion.identity,
                _rempartContainer
            );

            rempart.SetRempartBlock(rempartBlock);
            rempart.gridCell = gridCell;
            rempart.rempartsManager = this;
            gridCell.rempart = rempart;

            // Now we update all surrounding remparts
            RefreshRempartAroundCoordinates(gridCell.coords);
        }

        public void BuildRempartReworked(GridCellModel gridCell)
        {
            Vector3 worldPosition = _gridManager.gridModel.GetCellWorldPositionFromCoordinates(
                gridCell.coords
            );

            Rempart rempart = UnityEngine.Object.Instantiate(
                _prefabRempart,
                worldPosition,
                Quaternion.identity,
                _rempartContainer
            );

            rempart.gridCell = gridCell;
            rempart.rempartsManager = this;
            gridCell.rempart = rempart;
        }

        public void DestroyRempart(Rempart rempart)
        {
            rempart.gridCell.rempart = null;
            RefreshRempartAroundCoordinates(rempart.gridCell.coords);
            Destroy(rempart.gameObject);
        }

        public void DestroyRempartReworked(Rempart rempart)
        {
            rempart.gridCell.rempart = null;
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

                if (gridCell == null || gridCell.rempart == null)
                    bitmask += 0;
                else
                    bitmask += 1;
            }

            // converting to integer
            // Debug.Log(bitmask);
            int enumValue = Convert.ToInt32(bitmask, 2);
            // Debug.Log(enumValue);

            return enumValue;
        }

        public void RefreshRempartAroundCoordinates(Vector2Int coords)
        {
            for (int i = 0; i < TilesetUtils.neighboorsCoordinatesFour.Count; i++)
            {
                Vector2Int offset = TilesetUtils.neighboorsCoordinatesFour[i];
                GridCellModel gridCell =
                    _gridManager.gridModel.GetCellFromCoordinates<GridCellModel>(
                        new Vector2Int(coords.x + offset.x, coords.y + offset.y)
                    );
                // Debug.Log($"Coords {offset} \r Rempart : {gridCell.rempart == null}");
                if (gridCell == null || gridCell.rempart == null)
                    continue;
                else
                    gridCell.rempart.SetRempartBlock(GetRempartBlockFromCoords(gridCell.coords));
            }
        }

        public RempartBlock GetRempartBlockFromCoords(Vector2Int coords)
        {
            TilesetTypeFour type = (TilesetTypeFour)GetRempartNeighboorsFromCoords(coords);

            RempartBlock block = _rempartBlocks.Find(item => item.type == type);

            if (block.mesh != null)
                return block;
            else
            {
                Debug.LogWarning("Couldn't GetRempartMeshFromCoord");
                return new RempartBlock();
            }
        }

		#endregion
    }
}
