using UnityEngine;

namespace TideDefense
{
    using System.Collections.Generic;

    public class WashedUpObjectSequencerState : ASequencerState
    {
		#region Fields

		[Header("Object")]
        [SerializeField]
        private List<WashedUpObject> _washedUpObjects = null;

		#endregion

		#region Methods

		#region StateMachineBehaviour

        override public void OnStateEnter(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex
        )
        {
            foreach (WashedUpObject washedUp in _washedUpObjects)
                _sequencerChannel.onCreatedWashedUpObject(washedUp);   

            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        #endregion

		#endregion
    }
}
