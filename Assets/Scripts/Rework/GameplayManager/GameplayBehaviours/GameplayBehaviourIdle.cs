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
            _gameplayManager.UIChannel.onRefreshControlHints(ControlHintType.None);            
        }

        public override void Deactivate()
        {
            // Debug.Log($"Deactivate {this}");
        }
        

    }
}
