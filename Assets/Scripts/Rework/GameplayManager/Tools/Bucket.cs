using PierreMizzi.MouseInteractable;
using UnityEngine;

namespace TideDefense
{
    public class Bucket : MonoBehaviour, IClickable
    {

		#region Fields

		[SerializeField] private GameplayChannel _gameplayChannel = null;

		#endregion

		#region Methods

		#endregion

		#region IClickable

		public void OnLeftClick(RaycastHit hit)
		{
			_gameplayChannel.onClickBucket.Invoke();
		}

		#endregion

	}
}
