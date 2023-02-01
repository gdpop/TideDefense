using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TideDefense
{
    public class FloatingSequencerState : StateMachineBehaviour
    {
        #region Fields

        #region Behviour


        [Header("Behaviour")]
        [SerializeField]
        protected FloatingSequencerChannel _sequencerChannel = null;

        [SerializeField]
        protected FloatingObjectType _type = FloatingObjectType.None;
        public FloatingObjectType type
        {
            get { return _type; }
        }

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
            CreateFloatingObject();
            StartDelay();
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

        #region Behaviour

        protected virtual void CreateFloatingObject() { }

        #endregion

        #region Delay

        protected void StartDelay()
        {
            _delay = Random.Range(_minDelay, _maxDelay);
            DOVirtual.DelayedCall(_delay, ExitState);
        }

        protected void ExitState()
        {
            Debug.Log("Exit state");
        }

        #endregion

        #endregion
    }
}
