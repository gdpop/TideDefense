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

        #endregion

        #endregion

        #region Methods

        #endregion
    }
}
