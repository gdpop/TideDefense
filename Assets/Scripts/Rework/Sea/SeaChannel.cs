using System.Collections.Generic;
using UnityEngine;

namespace TideDefense
{
    [CreateAssetMenu(fileName = "SeaChannel", menuName = "TideDefense/SeaChannel", order = 0)]
    public class SeaChannel : ScriptableObject
    {
        #region Tide Settings


        [Header("Tide Settings")]
        /*
            Fork of time between two different waves
        */
        [SerializeField]
        private float _minTideLevel = 3.5f;

        public float minTideLevel
        {
            get { return _minTideLevel; }
        }

        [SerializeField]
        private float _maxTideLevel = 6.5f;

        public float maxTideLevel
        {
            get { return _maxTideLevel; }
        }

        #endregion

        #region Wave Settings

        [Header("Wave Settings")]
        [SerializeField]
        private AnimationCurve _waveCrashEase = null;
        public AnimationCurve waveCrashEase
        {
            get { return _waveCrashEase; }
            set { _waveCrashEase = value; }
        }

        [SerializeField]
        private float _waveCrashDuration = 8f;
        public float waveCrashDuration
        {
            get { return _waveCrashDuration; }
            set { _waveCrashDuration = value; }
        }

        [SerializeField]
        private float _minDelayBetweenWaves = 1f;
        public float minDelayBetweenWaves
        {
            get { return _minDelayBetweenWaves; }
        }

        [SerializeField]
        private float _maxDelayBewteenWaves = 2f;
        public float maxDelayBewteenWaves
        {
            get { return _maxDelayBewteenWaves; }
        }

        [SerializeField]
        private float _minWaveStrength = 0.5f;
        public float minWaveStrength
        {
            get { return _minWaveStrength; }
            set { _minWaveStrength = value; }
        }

        [SerializeField]
        private float _maxWaveStrength = 1.5f;
        public float maxWaveStrength
        {
            get { return _maxWaveStrength; }
            set { _maxWaveStrength = value; }
        }

        #endregion

        #region Wave Segment Settings

        [Header("Wave Segments Settings")]
        /// <summary>
        /// This is the minimal delay between two WaveSegmentAnimation
        ///</summary>
        [SerializeField]
        private float _minDelayWaveSegments = 0.1f;

        public float minDelayWaveSegments
        {
            get { return _minDelayWaveSegments; }
        }

        /// <summary>
        /// This is the maximal delay between two WaveSegmentAnimation
        ///</summary>
        [SerializeField]
        private float _maxDelayWaveSegments = 0.3f;

        public float maxDelayWaveSegments
        {
            get { return _maxDelayWaveSegments; }
        }

        [SerializeField]
        private AnimationCurve _damageDealtByWave;
        public AnimationCurve damageDealtByWave
        {
            get { return _damageDealtByWave; }
        }

        #endregion
    }
}
