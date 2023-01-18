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
            _gameplayManager.bucket.DisplayDiggableHints();

            if (_gameplayManager.gameplayChannel != null)
            {
                _gameplayManager.gameplayChannel.onClickGrid += CallbackOnClickGrid;
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _gameplayManager.bucket.HideDiggableHints();

            if (_gameplayManager.gameplayChannel != null)
            {
                _gameplayManager.gameplayChannel.onClickGrid -= CallbackOnClickGrid;
            }
        }

        public virtual void CallbackOnClickGrid(GridCell gridCell, RaycastHit hit)
        {
            if (CheckGridCellNeighboorToBucket(gridCell))
                Debug.Log("I fill the bucket !");
            else
                _gameplayManager.DropTool(_gameplayManager.shovel, gridCell);
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
