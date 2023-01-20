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

            if (_gameplayManager.bucket.isFull)
                _gameplayManager.gridManager.DisplayBuildableHints();
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

        public virtual void CallbackOnClickGrid(GridCell gridCell, RaycastHit hit)
        {
            _gameplayManager.DropTool(_gameplayManager.bucket, gridCell);
        }
    }
}
