using UnityEngine;

namespace TideDefense
{
    public class MessageBottleSequencerState : FloatingObjectSequencerState
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

        #endregion

		#endregion
    }
}
