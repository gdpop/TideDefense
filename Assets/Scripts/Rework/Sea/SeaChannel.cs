using System.Collections.Generic;
using UnityEngine;

namespace TideDefense
{
    [CreateAssetMenu(fileName = "SeaChannel", menuName = "TideDefense/SeaChannel", order = 0)]
    public class SeaChannel : ScriptableObject
    {
        #region Tide Settings


        [Header("Tide Settings")]
        
        [SerializeField]
        private float _minTideBeachCoverage = 3.5f;

        public float minTideBeachCoverage
        {
            get { return _minTideBeachCoverage; }
        }

        [SerializeField]
        private float _maxTideBeachCoverage = 6.5f;

        public float maxTideBeachCoverage
        {
            get { return _maxTideBeachCoverage; }
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

        [HideInInspector]
        private List<string> _waveSoundDataIDs = new List<string>();
        public List<string> waveSoundDataIDs { get { return _waveSoundDataIDs; } }

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

        [SerializeField] private AnimationCurve _strengthIndividualWaveSegment = null;
        public AnimationCurve strengthIndividualWaveSegment { get { return _strengthIndividualWaveSegment; } set { _strengthIndividualWaveSegment = value; } }

        [SerializeField]
        private AnimationCurve _damageDealtByWave;
        public AnimationCurve damageDealtByWave
        {
            get { return _damageDealtByWave; }
        }

        #endregion

        private void OnEnable() {
            _waveSoundDataIDs = new List<string>(){
                SoundDataIDStatic.WAVE01,
                SoundDataIDStatic.WAVE02,
                SoundDataIDStatic.WAVE03,
                SoundDataIDStatic.WAVE04,
                SoundDataIDStatic.WAVE05,
            };
        }
    }
}
