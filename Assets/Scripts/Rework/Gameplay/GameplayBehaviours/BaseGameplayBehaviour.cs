using System;
using UnityEngine;

namespace TideDefense
{
    public class BaseGameplayBehaviour
    {
        #region Behaviour

        protected GameplayManager _gameplayManager = null;

        public BeachToolType state
        {
            get { return BeachToolType.None; }
        }

        public BaseGameplayBehaviour() { }

        public BaseGameplayBehaviour(GameplayManager manager)
        {
            _gameplayManager = manager;
        }

        [Obsolete]
        public virtual void Initialize(GameplayManager manager) { }

        public virtual void Activate()
        {
            // Debug.Log($"Activated {this}");
        }

        public virtual void Deactivate()
        {
            // Debug.Log($"Deactivate {this}");
        }

        #endregion

        #region Callbacks

        #endregion
    }
}
