using UnityEngine;

namespace TideDefense
{
    public class MessageBottleSequencerState : FloatingSequencerState
    {
		#region Fields

		[Header("Message Bottle")]
        [SerializeField]
        private MessageBottleData _data = null;

		#endregion

		#region Methods

		#region StateMachineBehaviour

        override public void OnStateEnter(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex
        )
        {
            Debug.Log("Enter - MessageBottleSequencerState");
            base.OnStateEnter(animator, stateInfo, layerIndex);
            _sequencerChannel.onCreateMessageBottle.Invoke(_data);
        }

        override public void OnStateExit(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex
        ) { }

        override public void OnStateUpdate(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex
        ) { }

        #endregion

		#endregion
    }
}
