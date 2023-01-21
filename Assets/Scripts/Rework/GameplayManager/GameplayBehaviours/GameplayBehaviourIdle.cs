using UnityEngine;

namespace TideDefense
{
    public class GameplayBehaviourIdle : BaseGameplayBehaviour
    {
        public new ToolType state
        {
            get { return ToolType.None; }
        }

        public GameplayBehaviourIdle(GameplayManager manager)
        {
            _gameplayManager = manager;
        }

        public override void Activate()
        {
            base.Activate();
            if (_gameplayManager.gameplayChannel != null)
            {
                _gameplayManager.gameplayChannel.onClickGrid += CallbackOnClickGrid;
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            if (_gameplayManager.gameplayChannel != null)
            {
                _gameplayManager.gameplayChannel.onClickGrid -= CallbackOnClickGrid;
            }
        }

        public virtual void CallbackOnClickGrid(GridCellModel gridCell, RaycastHit hit)
        {
            _gameplayManager.rempartsManager.BuildRempart(gridCell);
        }
    }
}
