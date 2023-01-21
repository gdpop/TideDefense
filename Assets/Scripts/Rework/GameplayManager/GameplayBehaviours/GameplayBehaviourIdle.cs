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

    }
}
