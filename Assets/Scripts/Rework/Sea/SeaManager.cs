using UnityEngine;
using System;

namespace TideDefense
{
    public class SeaManager : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private TimeChannel _timeChannel = null;

        [SerializeField]
        private Transform _seaTransform = null;

        #region Tide

        [SerializeField]
        private float _minTideLevel = 0f;

        [SerializeField]
        private float _maxTideLevel = 1f;

        private float _tideProgress = 0f;
        private bool _tidePhase = true;
        private float _tidePhaseProgress = 0f;

        /// <summary>
        /// Exprimé en m/s
        ///</summary>
        [SerializeField]
        private float _tideProgressSpeed = 0.2f;

        #endregion

        #endregion

        #region Methods

        #endregion



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
            // Debug.Log(currentDeltaTime);

            _tideProgress += currentDeltaTime * _tideProgressSpeed;

            _tidePhase = Convert.ToBoolean(1 - (int)Mathf.Floor(_tideProgress % 2));
            _tidePhaseProgress = (1.0f / Mathf.PI) * Mathf.Acos(Mathf.Sin(Mathf.PI * (_tideProgress + 0.5f)));

            // Debug.Log($"_tidePhase : {_tidePhase}");
            Debug.Log($"_tidePhase : {_tidePhase} | _tidePhaseProgress : {_tidePhaseProgress}");

            _seaTransform.position = new Vector3(0f, Mathf.Lerp(_minTideLevel, _maxTideLevel, _tidePhaseProgress), 0f);
        }

        protected void CallbackUpdateCurrentTime(float currentTime)
        {
            // Debug.Log(currentTime);
        }
    }
}
