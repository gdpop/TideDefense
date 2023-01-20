using PierreMizzi.TilesetUtils;
using UnityEngine;

namespace TideDefense
{
    public class GameplayBehaviourShovel : BaseGameplayBehaviour
    {
        public new ToolType state
        {
            get { return ToolType.Shovel; }
        }

        public GameplayBehaviourShovel(GameplayManager manager)
        {
            _gameplayManager = manager;
        }

        public override void Activate()
        {
            base.Activate();
            _gameplayManager.gridManager.DisplayDiggableHints();

            if (_gameplayManager.gameplayChannel != null)
            {
                _gameplayManager.gameplayChannel.onClickGrid += CallbackOnClickGrid;
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _gameplayManager.gridManager.HideDiggableHints();

            if (_gameplayManager.gameplayChannel != null)
            {
                _gameplayManager.gameplayChannel.onClickGrid -= CallbackOnClickGrid;
            }
        }

        public virtual void CallbackOnClickGrid(GridCell gridCell, RaycastHit hit)
        {
            if (CheckGridCellNeighboorToBucket(gridCell))
            {
                FillBucket(gridCell, hit);
            }
            else
                _gameplayManager.DropTool(_gameplayManager.shovel, gridCell);
        }

        public void FillBucket(GridCell gridCell, RaycastHit hit)
        {
            float wetness = _gameplayManager.seaManager.beach.GetWetnessFromWorldPosition(
                hit.point
            );

            SandWaterFilling filling = new SandWaterFilling(
                _gameplayManager.shovelFillingQuantity,
                SandWaterFilling.GetSandConcentrationFromWetness(wetness)
            );
            // Debug.Log("Filling");
            // Debug.Log(filling.ToString());
            _gameplayManager.bucket.FillBucket(filling);
        }

        /// <summary>
        /// Verify if the clicked GridCell while grabbing the shovel is next to the bucket
        /// If it's next to the bucket, we fill it
        /// </summary>
        public virtual bool CheckGridCellNeighboorToBucket(GridCell clickedgridCell)
        {
            Vector2Int checkedCoords = new Vector2Int();
            foreach (Vector2Int offset in TilesetUtils.neighboorsCoordinatesEight)
            {
                checkedCoords = _gameplayManager.bucket.currentGridCell.coords + offset;

                if (checkedCoords == clickedgridCell.coords)
                    return true;
            }
            return false;
        }
    }
}
