using UnityEngine;
using System.Collections.Generic;

namespace TideDefense
{

    public class FloatingObjectSequencerState : ASequencerState
    {


        public List<FloatingObject> _floatingObjects = new List<FloatingObject>();

        #region Methods

        override public void OnStateEnter(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex
        )
        {
            foreach (FloatingObject floatingObject in _floatingObjects)
               _sequencerChannel.onCreateFloatingObject.Invoke(floatingObject);

            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        #endregion
    }
}
