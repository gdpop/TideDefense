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
                _gameplayManager.gameplayChannel.onLeftClickGrid += CallbackLeftClickGrid;
                _gameplayManager.gameplayChannel.onRightClickGrid += CallbackRightClickGrid;
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
                _gameplayManager.gameplayChannel.onLeftClickGrid -= CallbackLeftClickGrid;
                _gameplayManager.gameplayChannel.onRightClickGrid -= CallbackRightClickGrid;
            }

            _gameplayManager.gridManager.HideBuildableHints();
        }

        public virtual void CallbackLeftClickGrid(GridCellModel gridCell, RaycastHit hit)
        {
            if (gridCell.isEmpty)
            {
                _gameplayManager.DropTool(gridCell);
            }
        }

        public virtual void CallbackRightClickGrid(GridCellModel gridCell, RaycastHit hit)
        {
            if (currentMould.isFull)
            {
                _gameplayManager.fortificationManager.CastMould(
                    currentMould,
                    gridCell,
                    currentMould.content.sandConcentration
                );
                currentMould.Empty();
                _gameplayManager.UIChannel.onHideControlHint.Invoke(ControlHintType.BuildSandTower);
                _gameplayManager.gridManager.HideBuildableHints();
            }
        }
    }
}
