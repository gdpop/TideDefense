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
            _gameplayManager.UIChannel.onHideAllControlHint.Invoke();
            _gameplayManager.UIChannel.onDisplayControlHint.Invoke(ControlHintType.RotateSphericalCamera);
            _gameplayManager.gameplayChannel.onSetActiveSphericalCamera.Invoke(true);
        }

        public override void Deactivate()
        {
            // Debug.Log($"Deactivate {this}");
        }
        

    }
}
