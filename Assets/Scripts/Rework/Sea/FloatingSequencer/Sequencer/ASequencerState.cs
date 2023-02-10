
namespace TideDefense

{

	using UnityEngine;

	public abstract class ASequencerState : StateMachineBehaviour {
		#region Fields 

		[Header("Behaviour")]
		[SerializeField]
		protected SequencerChannel _sequencerChannel = null;
		public static string TRIGGER_NEXT = "Next";

		#endregion 
	}
}