namespace TideDefense
{
    public class HUDManager : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private TimeManager _timeManager = null;

        [SerializeField]
        private ToggleButton _pausePlayButton = null;

		#endregion

		#region Methods

		#region MonoBehaviour

        protected void Start()
        {
            if (_timeManager != null)
            {
                _timeManager.onSetTimeSpeedStopped += CallbackOnSetTimeSpeedStopped;
                _timeManager.onSetTimeSpeedNormal += CallbackOnSetTimeSpeedNormal;
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
