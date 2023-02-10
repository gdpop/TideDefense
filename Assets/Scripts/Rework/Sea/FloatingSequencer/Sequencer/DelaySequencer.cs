using DG.Tweening;
using UnityEngine;

namespace TideDefense
{
    public class DelaySequencer : StateMachineBehaviour
    {
        #region Fields

        #region Behviour

        [Header("Behaviour")]
        [SerializeField]
        protected float _minDelay = 1f;

        [SerializeField]
        protected float _maxDelay = 1f;

        protected float _delay = 0f;

        #endregion

        #endregion

        #region Methods

        #region StateMachineBehaviour

        override public void OnStateEnter(
            Animator animator,
            AnimatorStateInfo stateInfo,
            int layerIndex
        )
        {
            StartDelay(animator);
        }

        #endregion

        #region Delay

        protected void StartDelay(Animator animator)
        {
            _delay = Random.Range(_minDelay, _maxDelay);
            DOVirtual.DelayedCall(
                _delay,
                () =>
                {
                    animator.SetTrigger(ASequencerState.TRIGGER_NEXT);
                }
            );
        }

        #endregion

        #endregion
    }
}
