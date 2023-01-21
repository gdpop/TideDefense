using UnityEngine;
using System;

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
        private float _fastTimeSpeed = 1;
        public float fastTimeSpeed
        {
            get { return _fastTimeSpeed; }
        }

        public FloatDelegate onUpdateCurrentDeltaTime;
        public FloatDelegate onUpdateCurrentTime;

        public Action onSetTimeSpeedStopped = null;
        public Action onSetTimeSpeedNormal = null;
        public Action onSetTimeSpeedFast = null;

        protected void OnEnable()
        {
            onUpdateCurrentDeltaTime = (float value) => { };
            onUpdateCurrentTime = (float value) => { };

            onSetTimeSpeedStopped = ()=>{};
            onSetTimeSpeedNormal = ()=>{};
            onSetTimeSpeedFast = ()=>{};
        }

        public void InvokeSetTimeStopped()
        {
            onSetTimeSpeedStopped.Invoke();
        }

        public void InvokeSetTimeNormal()
        {
            onSetTimeSpeedNormal.Invoke();
        }
    }
}
