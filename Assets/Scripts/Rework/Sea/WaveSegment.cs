namespace TideDefense
{
    using UnityEngine;
    using DG.Tweening;

    public class WaveSegment : MonoBehaviour
    {
#region Fields

        [SerializeField]
        private Transform _visualTransform = null;

        private Vector3 _visualLocalScale;

        private int _segmentIndex = 0;

#endregion

#region Methods

        public void Start()
        {
            _visualLocalScale = _visualTransform.localScale;
            _segmentIndex = transform.GetSiblingIndex();
        }

        public void CrashOnBeach(int firstSegmentIndex, float delay, float strength)
        {
            float totalDelay = Mathf.Abs(_segmentIndex - firstSegmentIndex) * delay;

            DOVirtual
                .Float(
                    0f,
                    Mathf.PI,
                    5f,
                    (float value) =>
                    {
                        _visualLocalScale.z = Mathf.Lerp(0f, strength, Mathf.Sin(value));
                        _visualTransform.localScale = _visualLocalScale;
                    }
                )
                .SetEase(Ease.InOutSine)
                .SetDelay(totalDelay);
        }
#endregion
    }
}
