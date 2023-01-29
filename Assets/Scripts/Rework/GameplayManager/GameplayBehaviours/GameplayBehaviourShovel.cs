using PierreMizzi.TilesetUtils;
using UnityEngine;
using VirtuoseReality.Extension.AudioManager;
using VirtuoseReality.Helpers;

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
            _gameplayManager.DisplayDiggableHints();

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
            ContainerTool containerTool = CheckGridCellNeighboorToContainer(gridCell);

            if (containerTool != null)
            {
                SoundManager.PlaySound(SoundDataIDStatic.SHOVEL_DIG);

                FillContainerTool(containerTool, hit);
            }
            else if (gridCell.isEmpty)
                _gameplayManager.DropTool(_gameplayManager.currentTool, gridCell);
        }

        public void FillContainerTool(ContainerTool containerTool, RaycastHit hit)
        {
            float wetness = _gameplayManager.seaManager.beach.GetWetnessFromWorldPosition(
                hit.point
            );

            SandWaterFilling filling = new SandWaterFilling(
                _gameplayManager.shovelFillingQuantity,
                SandWaterFilling.GetSandConcentrationFromWetness(wetness)
            );
            containerTool.Fill(filling);
        }

        public ContainerTool CheckGridCellNeighboorToContainer(GridCellModel clickedgridCell)
        {
            Vector2Int checkedCoords = new Vector2Int();
            GridCellModel cellModel;
            foreach (Vector2Int offset in TilesetUtils.neighboorsCoordinatesEight)
            {
                checkedCoords = clickedgridCell.coords + offset;
                cellModel =
                    _gameplayManager.gridManager.gridModel.GetCellFromCoordinates<GridCellModel>(
                        checkedCoords
                    );
                if (
                    cellModel != null
                    && cellModel.currentTool != null
                    && BitMaskHelper.CheckMask(
                        (int)cellModel.currentTool.toolType,
                        (int)BeachToolType.Container
                    )
                )
                {
                    return (ContainerTool)cellModel.currentTool;
                }
            }
            return null;
        }
    }
}
