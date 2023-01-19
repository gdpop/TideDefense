using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TideDefense
{
    public class SeaManager : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private TimeChannel _timeChannel = null;

        [SerializeField]
        private SeaChannel _seaChannel = null;

        [SerializeField]
        private Transform _seaTransform = null;

		#region Beach

        [SerializeField] private Beach _beach = null;

        [SerializeField]
        private float _beachSlope = 5f;

        [SerializeField]
        private Transform _beachBottom = null;

		#endregion

		#region Tide

        /// <summary>
        ///	Small distance so the visual object of the sea goes into the beach
        /// </summary>
        [SerializeField]
        private float _seaSpreadOffset = 0.2f;

        /// <summary> 
        /// The Level of the tide goes along the slope of the beach. 
        /// It's a non-normalized, non-negative value expressed in meter 
        /// </summary>
        private float _currentTideLevel = 0f;

        /// <summary>
        /// "Time" value for the tide
        ///</summary>
        private float _tideProgress = 0f;

        /// <summary>
        /// Current phase of the tide : Ascending = true, descending = false
        ///</summary>
        private bool _tidePhase = true;

        /// <summary>
        /// Normalized value to determine the current progress of the phase
        /// For exemple when ascending, 0 = _minTideLevel, 1 = _maxTideLevel
        ///</summary>
        private float _tidePhaseProgress = 0f;

        /// <summary>
        /// Expressed en m/s
        ///</summary>
        [SerializeField]
        private float _tideProgressSpeed = 0.2f;

        /// <summary>
        ///	Position of the edge of the tide
        /// </summary>
        private Vector3 _currentTidePosition;
        public Vector3 currentTidePosition
        {
            get { return _currentTidePosition; }
        }

		#endregion

		#region Wave

        [Header("Wave")]
        [SerializeField]
        private GameObject _wavePrefab = null;

        private Wave _currentWave = null;

		#endregion

        #region Wetness

        private float[] _beachCoverageSegments = new float[12];
            
        #endregion

		#endregion

		#region Methods

		#region MonoBehaviour

        protected void Awake()
        {
        }

        protected void Start()
        {
            if (_timeChannel != null)
            {
                _timeChannel.onUpdateCurrentDeltaTime += CallbackUpdateCurrentDeltaTime;
            }

			StartCoroutine("DelayBetweenWaveBehaviour");
        }

        private void LateUpdate() {
            UpdateBeachCoveragePerSegment();
        }

		#endregion

		#region Tide

        /// <summary> 
        /// Mainly manages the tide ascending and descending motion.
        /// </summary>
        protected void CallbackUpdateCurrentDeltaTime(float currentDeltaTime)
        {
            _tideProgress += currentDeltaTime * _tideProgressSpeed;

            // Determine if it's ascending or descending now
            _tidePhase = Convert.ToBoolean(1 - (int)Mathf.Floor(_tideProgress % 2));

            // Compute the normalized progress of the current phase
            _tidePhaseProgress =
                (1.0f / Mathf.PI) * Mathf.Acos(Mathf.Sin(Mathf.PI * (_tideProgress + 0.5f)));

            // Debug.Log($"_tidePhase : {_tidePhase} | _tidePhaseProgress : {_tidePhaseProgress}");

            // Update tide level
            _currentTideLevel = Mathf.Lerp(
                _seaChannel.minTideLevel,
                _seaChannel.maxTideLevel,
                _tidePhaseProgress
            );

            // Move the sea up and down, just like the surface would do in real life
            _seaTransform.position =
                _beachBottom.position
                + new Vector3(0f, TideProgressToSeaHeight(_currentTideLevel), 0f);

            _seaTransform.localScale = new Vector3(
                _seaTransform.localScale.x,
                1f,
                TideProgressToSeaSpread(_currentTideLevel)
            );

            _currentTidePosition = _beachBottom.position + _beachBottom.forward * _currentTideLevel;
            _currentTidePosition.x = 0;
        }

        /// <summary>
        /// Give the height of the sea along the y axis when given the tide level
        ///</summary>
        private float TideProgressToSeaHeight(float tideLevel)
        {
            return Mathf.Cos(Mathf.Deg2Rad * (90.0f - _beachSlope)) * tideLevel;
        }

        /// <summary>
        ///	Give the spread(scale) of the sea when given the tide level
        ///</summary>
        private float TideProgressToSeaSpread(float tideLevel)
        {
            return _seaSpreadOffset + Mathf.Sin(Mathf.Deg2Rad * (90.0f - _beachSlope)) * tideLevel;
        }

		#endregion

		#region Wave

        /// <summary> 
        /// Wait a random time between two waves
        /// </summary>
		public IEnumerator DelayBetweenWaveBehaviour()
		{
			float randomDelay = UnityEngine.Random.Range(_seaChannel.minDelayBetweenWaves, _seaChannel.maxDelayBewteenWaves);

			yield return new WaitForSeconds(randomDelay);

			CreateWave();

			yield return null;
		}

        /// <summary> 
        /// Creates a new wave that will crash on the beach
        /// </summary>
        private void CreateWave()
        {
            _currentWave = UnityEngine.Object.Instantiate(_wavePrefab).GetComponent<Wave>();
            _currentWave.Initialize(this);
            _currentWave.onDisappear += CallbackDestroyCurrentWave;
        }

        /// <summary> 
        /// Wave crashed and returned back into the sea
        /// </summary>
        public void CallbackDestroyCurrentWave()
        {
            _currentWave.onDisappear -= CallbackDestroyCurrentWave;
            Destroy(_currentWave.gameObject);
            _currentWave = null;
			StartCoroutine("DelayBetweenWaveBehaviour");
        }

		#endregion

		#endregion

        #region Wetness

        public void UpdateBeachCoveragePerSegment()
        {
            float beachCoveragePerSegment = 0f;
            for (int i = 0; i < _beachCoverageSegments.Length; i++)
            {
                beachCoveragePerSegment = _currentTideLevel;
                if(_currentWave != null)
                    beachCoveragePerSegment += _currentWave.GetBeachCoverageFromWaveSegment(i);
                    
                _beachCoverageSegments[i] = beachCoveragePerSegment;

            }
            // Debug.Log($"Segment[0] : beachCoverage with _tideLevel : {_beachCoverageSegments[0]}");
            _beach.UpdateWetness(_beachCoverageSegments);
        }
            
        #endregion
    }
}
