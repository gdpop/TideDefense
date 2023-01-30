using UnityEngine;

namespace TideDefense
{
    public class GameplayBehaviourIdle : BaseGameplayBehaviour
    {
        public new BeachToolType state
        {
            get { return BeachToolType.None; }
        }

        public GameplayBehaviourIdle(GameplayManager manager)
        {
            _gameplayManager = manager;
        }

        public override void Activate()
        {
            _gameplayManager.UIChannel.onHideAllControlHint.Invoke();
            _gameplayManager.UIChannel.onDisplayControlHint.Invoke(
                ControlHintType.RotateSphericalCamera
            );
            _gameplayManager.gameplayChannel.onSetActiveSphericalCamera.Invoke(true);

            if (_gameplayManager.gameplayChannel != null)
                _gameplayManager.gameplayChannel.onClickGrid += CallbackOnClickGrid;
        }

        private void CallbackOnClickGrid(GridCellModel gridCell, RaycastHit hit)
        {
            _gameplayManager.fortificationManager.BuildSandTower(gridCell, 0.75f);
        }

        public override void Deactivate()
        {
            if (_gameplayManager.gameplayChannel != null)
                _gameplayManager.gameplayChannel.onClickGrid -= CallbackOnClickGrid;
        }
    }
}
