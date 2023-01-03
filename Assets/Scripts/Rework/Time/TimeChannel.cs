using UnityEngine;

namespace TideDefense
{
    [CreateAssetMenu(
        fileName = "TimeChannel",
        menuName = "ScriptableObjects/TimeChannel",
        order = 1
    )]
    public class TimeChannel : ScriptableObject
    {
        [SerializeField]
        private float _normalTimeSpeed = 1;
        public float normalTimeSpeed
        {
            get { return _normalTimeSpeed; }
            set { _normalTimeSpeed = value; }
        }

        public FloatEvent onUpdateCurrentDeltaTime;
        public FloatEvent onUpdateCurrentTime;

        public Action onSetTimeSpeedStopped = null;
        public Action onSetTimeSpeedNormal = null;
        public Action onSetTimeSpeedFast = null;

        protected void OnEnable()
        {
            onUpdateCurrentDeltaTime = (float value) => { };
            onUpdateCurrentTime = (float value) => { };

            onSetTimeSpeedStopped = new Action();
            onSetTimeSpeedNormal = new Action();
            onSetTimeSpeedFast = new Action();
        }
    }
}
