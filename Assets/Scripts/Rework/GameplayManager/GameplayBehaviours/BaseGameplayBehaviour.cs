namespace TideDefense
{
    public class BaseGameplayBehaviour
    {
        #region Behaviour

        protected GameplayManager _gameplayManager = null;

        public ToolType state
        {
            get { return ToolType.None; }
        }

        public virtual void Initialize(GameplayManager manager)
        {
            _gameplayManager = manager;
        }

        public virtual void Activate() { }

        public virtual void Deactivate() { }

        #endregion




        #region Callbacks

        #endregion
    }
}
