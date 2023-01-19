using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TideDefense
{
    public delegate void WaveSegmentDelegate(WaveSegment segment);

    public class Wave : MonoBehaviour
    {
		#region Fields

		[SerializeField] private SeaChannel _seaChannel = null;

        private SeaManager _seaManager = null;

        /// <summary>
        ///	Invoked whenever the wave has finished returning to the sea
        ///</summary>
        public Action onDisappear = null;

        [SerializeField]
        private List<WaveSegment> _waveSegments = new List<WaveSegment>();

        private int _amountWaveSegment = 0;

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
            _amountWaveSegment = _waveSegments.Count;
            CrashOnBeach();
        }

        [ContextMenu("CrashOnBeach")]
        public void CrashOnBeach()
        {
            // Simulate a wave with random delay between segments

            int firstSegmentIndex = UnityEngine.Random.Range(0, _amountWaveSegment);
            float delayWaveSegment = UnityEngine.Random.Range(
                _seaChannel.minDelayWaveSegments,
                _seaChannel.maxDelayWaveSegments
            );

            // Launch crashing animation of every WaveSegments
            foreach (WaveSegment segment in _waveSegments)
            {
                segment.CrashOnBeach(firstSegmentIndex, delayWaveSegment, 1f);
                segment.onDisappear += CallbackWaveSegmentDisappear;
            }

            // Initialize variable to check when it's gonna crash
            _amountWaveSegmentDisappeared = 0;
        }

        private void CallbackWaveSegmentDisappear(WaveSegment segment)
        {
            segment.onDisappear -= CallbackWaveSegmentDisappear;
            _amountWaveSegmentDisappeared++;

            if (_amountWaveSegmentDisappeared == _amountWaveSegment)
            {
                onDisappear.Invoke();
            }
        }

        public float GetBeachCoverageFromWaveSegment(int waveSegmentIndex)
        {
            if(0 <= waveSegmentIndex && waveSegmentIndex < _waveSegments.Count)
            {
                return _waveSegments[waveSegmentIndex].beachCoverage;
            }else         
                return -1f;
        }

		#endregion
    }
}
