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

        public FloatEvent onUpdateCurrentDeltaTime;
        public FloatEvent onUpdateCurrentTime;

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
    }
}
