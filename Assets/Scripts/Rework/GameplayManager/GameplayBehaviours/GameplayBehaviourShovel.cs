using PierreMizzi.TilesetUtils;
using UnityEngine;
using VirtuoseReality.Extension.AudioManager;

namespace TideDefense
{
    public class GameplayBehaviourShovel : BaseGameplayBehaviour
    {
        public new BeachToolType state
        {
            get { return BeachToolType.Shovel; }
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
                //Invoke
                _gameplayManager.gameplayChannel.onSetActiveSphericalCamera.Invoke(false);

                // Subscribe
                _gameplayManager.gameplayChannel.onClickGrid += CallbackOnClickGrid;
            }

            _gameplayManager.UIChannel.onDisplayControlHint.Invoke(ControlHintType.DropTool);
            _gameplayManager.UIChannel.onHideControlHint.Invoke(
                ControlHintType.RotateSphericalCamera
            );

            if (!_gameplayManager.bucket.isFull)
                _gameplayManager.UIChannel.onDisplayControlHint.Invoke(ControlHintType.FillBucket);
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

        public virtual void CallbackOnClickGrid(GridCellModel gridCell, RaycastHit hit)
        {
            if (CheckGridCellNeighboorToBucket(gridCell))
            {
                SoundManager.PlaySound(SoundDataIDStatic.SHOVEL_DIG);

                FillBucket(gridCell, hit);
                if (_gameplayManager.bucket.isFull)
                    _gameplayManager.UIChannel.onHideControlHint.Invoke(ControlHintType.FillBucket);
            }
            else if (gridCell.isEmpty)
                _gameplayManager.DropTool(_gameplayManager.shovel, gridCell);
        }

        public void FillBucket(GridCellModel gridCell, RaycastHit hit)
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
            _gameplayManager.bucket.Fill(filling);
        }

        /// <summary>
        /// Verify if the clicked GridCell while grabbing the shovel is next to the bucket
        /// If it's next to the bucket, we fill it
        /// </summary>
        public virtual bool CheckGridCellNeighboorToBucket(GridCellModel clickedgridCell)
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
