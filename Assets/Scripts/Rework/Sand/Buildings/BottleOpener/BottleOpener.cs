namespace TideDefense
{
	using PierreMizzi.MouseInteractable;
	using System.Collections;
	using UnityEngine;
	
	public class BottleOpener : Building {
		
		#region Fields 


		
		private MessageBottle _messageBottle = null;

		[SerializeField] private HoldClickable _clickable = null;

		private IEnumerator _openBottleCoroutine = null;
		private IEnumerator _stunCoroutine = null;

		#endregion 
		
		#region Methods 

		public void AssignMessageBottle(MessageBottle messageBottle)
		{
			_messageBottle = messageBottle;
		}

		private void StartOpenBehaviour()
		{

		}

		private IEnumerator OpenBehaviour()
		{
			

			yield return null;
		}

		public override void InflictDamage(float damageTaken){}
		
		#endregion

	}
}