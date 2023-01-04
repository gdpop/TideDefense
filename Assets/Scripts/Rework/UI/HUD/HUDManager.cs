using UnityEngine;

namespace TideDefense
{
    public class HUDManager : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private TimeChannel _timeChannel = null;

        [SerializeField]
        private ToggleButton _pausePlayButton = null;

		#endregion

		#region Methods

		#region MonoBehaviour

        protected void Start()
        {
            if (_timeChannel != null)
            {
                _timeChannel.onSetTimeSpeedStopped += CallbackOnSetTimeSpeedStopped;
                _timeChannel.onSetTimeSpeedNormal += CallbackOnSetTimeSpeedNormal;
            }
        }

		#endregion

		#region Callabcks

        public void CallbackOnSetTimeSpeedStopped()
        {
            _pausePlayButton.SetStateFalse();
        }

        public void CallbackOnSetTimeSpeedNormal()
        {
            _pausePlayButton.SetStateTrue();
        }

        // 		public void CallbackOnSetTimeSpeedFast()
        // {

        // }

		#endregion

		#endregion
    }
}
