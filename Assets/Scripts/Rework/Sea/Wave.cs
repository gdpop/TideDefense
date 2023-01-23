using UnityEngine;
using System;
using System.Collections.Generic;
using CodesmithWorkshop.Useful;
using VirtuoseReality.Extension.AudioManager;

namespace TideDefense
{
    public delegate void WaveSegmentDelegate(WaveSegment segment);

    public class Wave : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private SeaChannel _seaChannel = null;

        private SeaManager _seaManager = null;

        /// <summary>
        ///	Invoked whenever the wave has finished returning to the sea
        ///</summary>
        public Action onDisappear = null;

        [SerializeField]
        private List<WaveSegment> _waveSegments = new List<WaveSegment>();

        public int amountWaveSegment
        {
            get 
            {
                return _waveSegments.Count;
            }
        }

        private bool _isCrashing = false;

        private int _amountWaveSegmentDisappeared = 0;


		#endregion

		#region Methods

		#region MonoBehaviour

        protected void LateUpdate()
        {
            if (_seaManager != null)
            {
                transform.position = _seaManager.currentTidePosition;
            }
        }

		#endregion

        public void Initialize(SeaManager seaManager)
        {
            _seaManager = seaManager;
            transform.position = _seaManager.currentTidePosition;
            CrashOnBeach();
        }

        [ContextMenu("CrashOnBeach")]
        public void CrashOnBeach()
        {
            // Simulate a wave with random delay between segments
            int firstSegmentIndex = UnityEngine.Random.Range(0, amountWaveSegment);
            float delayWaveSegment = UnityEngine.Random.Range(
                _seaChannel.minDelayWaveSegments,
                _seaChannel.maxDelayWaveSegments
            );

            // Set a random strength of the wave
            // Overal strength of the wave
            float randomWaveStrength = UnityEngine.Random.Range(
                _seaChannel.minWaveStrength,
                _seaChannel.maxWaveStrength
            );

            // Strength of individual segments
            int strongestSegmentIndex = UnityEngine.Random.Range(0, amountWaveSegment);

            // Launch crashing animation of every WaveSegments
            foreach (WaveSegment segment in _waveSegments)
            {
                segment.CrashOnBeach(
                    this,
                    firstSegmentIndex,
                    delayWaveSegment,
                    strongestSegmentIndex,
                    randomWaveStrength
                );
                segment.onDisappear += CallbackWaveSegmentDisappear;
            }

            // Initialize variable to check when it's gonna crash
            _amountWaveSegmentDisappeared = 0;

            PlayRandomWaveSound();
        }

        private void CallbackWaveSegmentDisappear(WaveSegment segment)
        {
            segment.onDisappear -= CallbackWaveSegmentDisappear;
            _amountWaveSegmentDisappeared++;

            if (_amountWaveSegmentDisappeared == amountWaveSegment)
            {
                onDisappear.Invoke();
            }
        }

        public float GetBeachCoverageFromWaveSegment(int waveSegmentIndex)
        {
            if (0 <= waveSegmentIndex && waveSegmentIndex < _waveSegments.Count)
            {
                return _waveSegments[waveSegmentIndex].beachCoverage;
            }
            else
                return -1f;
        }

		#endregion


        private void PlayRandomWaveSound()
        {
            string randomSoundDataID = UtilsClass.PickRandomInList<string>(_seaChannel.waveSoundDataIDs);
            SoundManager.PlaySound(randomSoundDataID);
        }
    }
}
