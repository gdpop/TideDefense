using UnityEngine;

namespace TideDefense
{
    public class ToggleButton : MonoBehaviour
    {
        [SerializeField]
        protected bool _currentState = false;

        [SerializeField]
        protected bool _initialState = true;

        public UnityEvent onStateTrue = null;
        public UnityEvent onStateFalse = null;

        #region Animator

        [SerializeField]
        protected Animator _animator = null;
        protected const string STATE_PARAM = "State";

        #endregion

        protected void Start()
        {
            if (_initialState)
                SetStateTrue();
            else
                SetStateFalse();
        }

        public virtual void SetStateFalse()
        {
            _currentState = false;
            SetAnimatorStateFalse();
        }

        public virtual void SetStateTrue()
        {
            _currentState = true;
            SetAnimatorStateTrue();
        }

        protected virtual void OnClick()
        {
            if (_currentState)
            {
                SetStateFalse();
                onStateFalse.Invoke();
            }
            else
            {
                SetStateTrue();
                onStateTrue.Invoke();
            }
        }

        #region Animator

        protected virtual void SetAnimatorStateTrue()
        {
            _animator.SetBool(STATE_PARAM, true);
        }

        protected virtual void SetAnimatorStateFalse()
        {
            _animator.SetBool(STATE_PARAM, false);
        }

        #endregion
    }
}
