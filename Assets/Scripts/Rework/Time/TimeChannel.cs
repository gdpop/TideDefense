using UnityEngine;

namespace TideDefense
{
    [CreateAssetMenu(fileName = "TimeChannel", menuName = "ScriptableObjects/TimeChannel", order = 1)]
    public class TimeChannel : ScriptableObject
    {
        [SerializeField] private float _normalTimeSpeed = 1;
        public float _normalTimeSpeed { get { return _normalTimeSpeed; } set { _normalTimeSpeed = value; } }

        public FloatEvent onUpdateCurrentDeltaTime = null;
        public FloatEvent onUpdateCurrentTime = null;

        protected void OnEnable()
        {
            onUpdateCurrentDeltaTime = new FloatEvent((float value)=>{});
            onUpdateCurrentTime = new FloatEvent((float value)=>{});
        }
    }
}