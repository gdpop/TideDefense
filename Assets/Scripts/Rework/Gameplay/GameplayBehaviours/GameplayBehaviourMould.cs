using System.Collections.Generic;
using UnityEngine;

namespace TideDefense
{
    public class GameplayBehaviourMould : BaseGameplayBehaviour
    {
        public new BeachToolType state
        {
            get { return BeachToolType.Bucket; }
        }

        public MouldTool currentMould
        {
            get { return (MouldTool)_gameplayManager.currentTool; }
        }

        public GameplayBehaviourMould(GameplayManager manager)
        {
            _gameplayManager = manager;
        }

        public override void Activate()
        {
            base.Activate();

            if (_gameplayManager.gameplayChannel != null)
            {
                //Invoke
                _gameplayManager.gameplayChannel.onSetActiveSphericalCamera.Invoke(false);

                // Subscribe
                _gameplayManager.gameplayChannel.onClickGrid += CallbackOnClickGrid;

                _gameplayManager.gameplayChannel.OnStartHoldClickGrid += CallbackStartHoldClickGrid;
                _gameplayManager.gameplayChannel.OnProgressHoldClickGrid +=
                    CallbackProgressHoldClickGrid;
                _gameplayManager.gameplayChannel.OnCompleteHoldClickGrid +=
                    CallbackCompleteHoldClickGrid;
                _gameplayManager.gameplayChannel.OnCancelHoldClickGrid +=
                    CallbackCancelHoldClickGrid;
            }
            if (_gameplayManager.UIChannel != null)
            {
                _gameplayManager.UIChannel.onDisplayControlHint.Invoke(ControlHintType.DropTool);
                _gameplayManager.UIChannel.onHideControlHint.Invoke(
                    ControlHintType.RotateSphericalCamera
                );
            }

            if (currentMould.isFull)
            {
                _gameplayManager.UIChannel.onDisplayControlHint.Invoke(
                    ControlHintType.BuildSandTower
                );
                _gameplayManager.gridManager.DisplayBuildableHints();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();

            if (_gameplayManager.gameplayChannel != null)
            {
                // Subscribe
                _gameplayManager.gameplayChannel.onClickGrid -= CallbackOnClickGrid;

                _gameplayManager.gameplayChannel.OnStartHoldClickGrid -= CallbackStartHoldClickGrid;
                _gameplayManager.gameplayChannel.OnProgressHoldClickGrid -=
                    CallbackProgressHoldClickGrid;
                _gameplayManager.gameplayChannel.OnCompleteHoldClickGrid -=
                    CallbackCompleteHoldClickGrid;
                _gameplayManager.gameplayChannel.OnCancelHoldClickGrid -=
                    CallbackCancelHoldClickGrid;
            }

            _gameplayManager.gridManager.HideBuildableHints();
        }

        public virtual void CallbackOnClickGrid(GridCellModel gridCell, RaycastHit hit)
        {
            if (gridCell.isEmpty)
            {
                _gameplayManager.DropTool(gridCell);
            }
        }

       #region HoldClick

        private void CallbackStartHoldClickGrid() { }

        private void CallbackProgressHoldClickGrid(float value) { }

        private void CallbackCompleteHoldClickGrid(GridCellModel clickedCell)
        {
            if (currentMould.isFull)
            {
                _gameplayManager.fortificationManager.CastMould(
                    currentMould,
                    clickedCell,
                    currentMould.content.sandConcentration
                );
                currentMould.Empty();
                _gameplayManager.UIChannel.onHideControlHint.Invoke(ControlHintType.BuildSandTower);
                _gameplayManager.gridManager.HideBuildableHints();
            }
        }

        private void CallbackCancelHoldClickGrid() { }

       #endregion
    }
}
