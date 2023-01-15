namespace TideDefense
{
    using UnityEngine;

    public class GameplayManager : MonoBehaviour
    {
		#region Fields


        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

        [SerializeField]
        private RempartsManager _rempartsManager = null;

		#endregion

		#region Methods

		#region MonoBehaviour

        private void Start()
        {
            if (_gameplayChannel != null)
                _gameplayChannel.onClickGrid += CallbackOnClickGrid;
        }

        private void OnDestroy()
        {
            if (_gameplayChannel != null)
                _gameplayChannel.onClickGrid -= CallbackOnClickGrid;
        }

		#endregion

        private void CallbackOnClickGrid(Vector2Int coords)
        {
            _rempartsManager.BuildRempart(coords);
        }

		#endregion
    }
}
