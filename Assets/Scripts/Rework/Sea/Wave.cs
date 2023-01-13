using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace TideDefense
{
    public class Wave : MonoBehaviour
    {
        #region Fields

        private SeaManager _seaManager = null;

        public Action onDisappear = null;

        [SerializeField]
        private List<WaveSegment> _waveSegments = new List<WaveSegment>();

        /// <summary> 
        /// This is the minimal delay between two 
        ///</summary>
        [SerializeField]
        private float _mindelayWaveSegments = 0.1f;

        [SerializeField]
        private float _maxdelayWaveSegments = 0.3f;

        #endregion

        #region Methods

        public void Initialize(SeaManager seaManager)
        {
            _seaManager = seaManager;
            transform.position = _seaManager.currentTidePosition;
            StartCoroutine("DisappearBehaviour");
        }

        [ContextMenu("CrashOnBeach")]
        public void CrashOnBeach()
        {
            // Simulate a wave with random delay between segments

            int firstSegmentIndex = UnityEngine.Random.Range(0, _waveSegments.Count);
            float delayWaveSegment = UnityEngine.Random.Range(_mindelayWaveSegments, _maxdelayWaveSegments);

            //

            foreach (WaveSegment segment in _waveSegments)
            {
                segment.CrashOnBeach(firstSegmentIndex, delayWaveSegment, 1f);
            }
        }

        protected void LateUpdate()
        {
            if (_seaManager != null)
            {
                transform.position = _seaManager.currentTidePosition;
            }
        }

        private IEnumerator DisappearBehaviour()
        {
            yield return new WaitForSeconds(2f);

            onDisappear.Invoke();

            yield return null;
        }

        #endregion
    }
}
