using System.Collections.Generic;
using UnityEngine;

namespace TideDefense
{
    public class GameplayBehaviourBucket : BaseGameplayBehaviour
    {
        public new ToolType state
        {
            get { return ToolType.Bucket; }
        }

        public GameplayBehaviourBucket(GameplayManager manager)
        {
            _gameplayManager = manager;
        }

        public override void Activate()
        {
            base.Activate();

            if (_gameplayManager.gameplayChannel != null)
            {
                _gameplayManager.gameplayChannel.onClickGrid += CallbackOnClickGrid;

                _gameplayManager.gameplayChannel.OnStartHoldClickGrid += CallbackStartHoldClickGrid;
                _gameplayManager.gameplayChannel.OnProgressHoldClickGrid +=
                    CallbackProgressHoldClickGrid;
                _gameplayManager.gameplayChannel.OnCompleteHoldClickGrid +=
                    CallbackCompleteHoldClickGrid;
                _gameplayManager.gameplayChannel.OnCancelHoldClickGrid +=
                    CallbackCancelHoldClickGrid;
            }

            ControlHintType[] hints = new ControlHintType[2];
            hints[0] = ControlHintType.DropTool;

            if (_gameplayManager.bucket.isFull)
            {
                hints[1] = ControlHintType.BuildSandTower;
                _gameplayManager.gridManager.DisplayBuildableHints();
            }
            
            _gameplayManager.UIChannel.onRefreshControlHints(hints);

        }

        public override void Deactivate()
        {
            base.Deactivate();

            if (_gameplayManager.gameplayChannel != null)
            {
                _gameplayManager.gameplayChannel.onClickGrid -= CallbackOnClickGrid;
            }

            _gameplayManager.gridManager.HideBuildableHints();
        }

        public virtual void CallbackOnClickGrid(GridCellModel gridCell, RaycastHit hit)
        {
            _gameplayManager.DropTool(_gameplayManager.bucket, gridCell);
        }

       #region HoldClick

        private void CallbackStartHoldClickGrid() { }

        private void CallbackProgressHoldClickGrid(float value) { }

        private void CallbackCompleteHoldClickGrid(GridCellModel clickedCell)
        {
            if (_gameplayManager.bucket.isFull)
            {
                _gameplayManager.rempartsManager.BuildRempartReworked(clickedCell);
                _gameplayManager.bucket.Empty();
            }
        }

        private void CallbackCancelHoldClickGrid() { }

       #endregion
    }
}
