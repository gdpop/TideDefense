using UnityEngine;
using System;

namespace TideDefense
{
    public class SeaManager : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private TimeChannel _timeChannel = null;

        private Transform _seaTransform = null;

        [SerializeField]
        private float _seaSpreadOffset = 0.2f;

		#region Beach

        [SerializeField]
        private float _beachSlope = 0f;

        [SerializeField]
        private Transform _beachBottom = null;

		#endregion

		#region Tide

        /*
            Level of the tide is a distance between the bottom of the ocean and a point on a beach.
            It goes along the beach, not the y axis
        */

        [SerializeField]
        private float _minTideLevel = 0.5f;

        [SerializeField]
        private float _maxTideLevel = 1.5f;

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


		#region MonoBehaviour

        protected void Awake()
        {
            _beachSlope = _beachBottom.transform.rotation.euleurAngles.x;
            Debug.Log("BeachSlope : " + _beachSlope);
        }

        protected void Start()
        {
            if (_timeChannel != null)
            {
                _timeChannel.onUpdateCurrentDeltaTime += CallbackUpdateCurrentDeltaTime;
            }
        }

		#endregion

        protected void CallbackUpdateCurrentDeltaTime(float currentDeltaTime)
        {
            _tideProgress += currentDeltaTime * _tideProgressSpeed;

            _tidePhase = Convert.ToBoolean(1 - (int)Mathf.Floor(_tideProgress % 2));
            _tidePhaseProgress =
                (1.0f / Mathf.PI) * Mathf.Acos(Mathf.Sin(Mathf.PI * (_tideProgress + 0.5f)));

            // Debug.Log($"_tidePhase : {_tidePhase} | _tidePhaseProgress : {_tidePhaseProgress}");

            _seaTransform.position = _beachBottom + new Vector3(
                0f,
                TideProgressToSeaHeight(_tideProgress),
                0f
            );

			_seaTransform.scale = new Vector3(
				1f,
				TideProgressToSeaSpread(_tideProgress),
				1f
			);
        }

        /// <summary>
        /// Give the height of the sea along the y axis when given the tide level
        ///</summary>
        private float TideProgressToSeaHeight(float tideLevel)
        {
            return Mathf.Cos(90.0f - _beachSlope) * tideLevel;
        }

        /// <summary>
        ///	Give the spread(scale) of the sea when given the tide level
        ///</summary>
        private float TideProgressToSeaSpread(float tideLevel)
        {
            return _seaSpreadOffset + Mathf.Sin(90.0f - _beachSlope) * tideLevel;
        }

		#endregion
    }
}
