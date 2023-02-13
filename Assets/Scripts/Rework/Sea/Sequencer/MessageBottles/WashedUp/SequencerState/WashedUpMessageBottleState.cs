using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TideDefense
{
    public class WashedUpMessageBottleState : ASequencerState
    {
        [SerializeField]
        private MessageBottleData _data = null;

        public override void OnStateEnter(
            Animator animator,
            AnimatorStateInfo animatorStateInfo,
            int layerIndex
        )
        {
            _sequencerChannel.onCreateFloatingMessageBottle.Invoke(_data);
            base.OnStateEnter(animator, animatorStateInfo, layerIndex);
        }
    }
}
