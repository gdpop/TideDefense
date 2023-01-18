using PierreMizzi.TilesetUtils;
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

        public virtual void CallbackOnClickGrid(GridCell gridCell, RaycastHit hit)
        {
            _gameplayManager.DropTool(_gameplayManager.bucket, gridCell);
        }

    }
}
