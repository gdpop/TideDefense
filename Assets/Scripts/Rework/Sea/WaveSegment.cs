namespace TideDefense
{
    using UnityEngine;
    using DG.Tweening;
    using System;

    public class WaveSegment : MonoBehaviour
    {
#region Fields


        [SerializeField]
        private SeaChannel _seaChannel = null;

        [SerializeField]
        private Transform _visualTransform = null;

        private Vector3 _visualLocalScale;

        private int _segmentIndex = 0;

        public WaveSegmentDelegate onDisappear = null;

#endregion

#region Methods

        public void CrashOnBeach(int firstSegmentIndex, float delay, float strength)
        {
            _visualLocalScale = _visualTransform.localScale;
            _segmentIndex = transform.GetSiblingIndex();

            float totalDelay = Mathf.Abs(_segmentIndex - firstSegmentIndex) * delay;

            DOVirtual
                .Float(
                    0f,
                    Mathf.PI,
                    _seaChannel.waveCrashDuration,
                    (float value) =>
                    {
                        _visualLocalScale.z = Mathf.Lerp(0f, strength, Mathf.Sin(value));
                        _visualTransform.localScale = _visualLocalScale;
                    }
                )
                .SetEase(_seaChannel.waveCrashEase)
                .SetDelay(totalDelay)
                .OnComplete(() =>
                {
                    onDisappear.Invoke(this);
                });
        }
#endregion
    }
}
