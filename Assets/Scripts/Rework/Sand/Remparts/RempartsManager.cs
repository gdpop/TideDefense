namespace TideDefense
{
    using UnityEngine;

    public class RempartsManager : MonoBehaviour
    {
		#region Fields
        [SerializeField]
        private GameplayChannel _gameplayChannel = null;
		#endregion

		#region Methods

        private void Start()
        {
            if (_gameplayChannel != null)
            {
                _gameplayChannel.onClickGrid += CallbackOnClickGrid;
            }
        }

        private void OnDestroy()
        {
            if (_gameplayChannel != null)
            {
                _gameplayChannel.onClickGrid -= CallbackOnClickGrid;
            }
        }

        private void CallbackOnClickGrid(Vector2 coords)
        {
            Debug.Log($"Clicked ! : {coords}");
        }

		#endregion
    }
}
