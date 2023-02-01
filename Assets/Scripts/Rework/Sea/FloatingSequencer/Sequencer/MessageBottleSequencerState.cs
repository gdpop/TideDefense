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
            _sequencerChannel.onCreateMessageBottle.Invoke(_data);
            base.OnStateEnter(animator, stateInfo, layerIndex);
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
