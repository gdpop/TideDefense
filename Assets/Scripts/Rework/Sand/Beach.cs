namespace TideDefense
{
    using PierreMizzi.MouseInteractable;
    using UnityEngine;

	// TODO : Refact parameters of the Beach : BeachSlope, BeachBottom ...
	// TODO : Separate visual and logical elements

    public class Beach : MonoBehaviour, IClickable
    {
		#region Fields

        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

		#endregion

		#region Methods

		#endregion

		#region IClickable

        public void onLeftClick(RaycastHit hit)
        {
			_gameplayChannel.onClickBeach.Invoke(hit);
        }

		#endregion
    }
}
