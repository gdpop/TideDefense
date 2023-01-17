using UnityEngine;

namespace TideDefense
{
    public class GameplayBehaviourShovel : BaseGameplayBehaviour
    {
        public new ToolType state
        {
            get { return ToolType.Shovel; }
        }

        public override void Activate()
        {
            if (_gameplayManager.gameplayChannel != null)
            {
                _gameplayManager.gameplayChannel.onClickGrid += CallbackOnClickGrid;
            }
        }

        public override void Deactivate()
        {
            if (_gameplayManager.gameplayChannel != null)
            {
                _gameplayManager.gameplayChannel.onClickGrid -= CallbackOnClickGrid;
            }
        }

        public virtual void CallbackOnClickGrid(GridCell gridCEll, RaycastHit hit) { }
    }
}
