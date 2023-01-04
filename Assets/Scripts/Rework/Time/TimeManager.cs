using UnityEngine;

namespace TideDefense
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField]
        private TimeChannel _timeChannel = null;

        private float _currentTime = 0f;
        private float _currentTimeSpeed = 1f;
        private float _currentDeltaTime = 0f;

        protected void Start()
        {
            if (_timeChannel != null)
            {
                _timeChannel.onSetTimeSpeedStopped += CallbackOnSetTimeSpeedStopped;
                _timeChannel.onSetTimeSpeedNormal += CallbackOnSetTimeSpeedNormal;
                _timeChannel.onSetTimeSpeedFast += CallbackOnSetTimeSpeedFast;
            }
        }

        protected void Update()
        {
            _currentDeltaTime = Time.deltaTime * _currentTimeSpeed;
            _timeChannel.onUpdateCurrentDeltaTime.Invoke(_currentDeltaTime);

            _currentTime += _currentDeltaTime;
            _timeChannel.onUpdateCurrentTime.Invoke(_currentTime);
        }

        #region Callabcks

        private void CallbackOnSetTimeSpeedStopped()
        {
            _currentTimeSpeed = 0f;
        }

        private void CallbackOnSetTimeSpeedNormal()
        {
            _currentTimeSpeed = 1f;
        }

        private void CallbackOnSetTimeSpeedFast()
        {
            _currentTimeSpeed = _timeChannel.fastTimeSpeed;
        }

        #endregion
    }
}
