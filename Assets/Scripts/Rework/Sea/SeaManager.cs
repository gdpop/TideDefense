UnityEngine;

namespace TideDefense
{
    public class SeaManager : MonoBehaviour
    {
        [SerializeField]
        private TimeChannel _timeChannel = null;

        #region MonoBehaviour


        protected void Start()
        {
            if (_timeChannel != null)
            {
                _timeChannel.onUpdateCurrentDeltaTime += CallbackUpdateCurrentDeltaTime;
                _timeChannel.onUpdateCurrentTime += CallbackUpdateCurrentTime;
            }
        }

        #endregion

        protected void CallbackUpdateCurrentDeltaTime(float currentDeltaTime)
        {
            Debug.Log(currentDeltaTime);
        }

        protected void CallbackUpdateCurrentTime(float currentTime)
        {
            Debug.Log(currentTime);
        }
    }
}
