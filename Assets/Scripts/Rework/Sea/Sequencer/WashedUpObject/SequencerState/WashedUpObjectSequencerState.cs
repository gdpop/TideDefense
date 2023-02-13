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
            {
                Debug.Log(washedUp.name);
                _sequencerChannel.onCreatedWashedUpObject.Invoke(washedUp);   
            }

            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        #endregion

		#endregion
    }
}
