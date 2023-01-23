namespace TideDefense
{
    using DG.Tweening;
    using UnityEngine;
    using VirtuoseReality.Rendering;

    public class FlagPole : MonoBehaviour
    {
		#region Fields

        private SandTower _rempart = null;

        private void Awake()
        {
            _flag.transform.localPosition = Vector3.zero;
        }

        public void Initialize(SandTower rempart)
        {
            _rempart = rempart;
        }

		#region Flag Color

        [Header("Color")]
        [SerializeField]
        private MaterialPropertyBlockModifier _materialPropertyBlock = null;

        private const string QUALITY_PROPERTY = "_Quality";

        public void RefreshFlagColor(float quality)
        {
            _materialPropertyBlock.SetFloat(QUALITY_PROPERTY, quality);
        }

		#endregion

		#region Flag Height

        [Header("Flag Height")]
        [SerializeField]
        private Transform _flag = null;

        [SerializeField]
        private Transform _lowAnchor = null;

        [SerializeField]
        private Transform _highAnchor = null;

        public void RaiseFlag()
        {
            _flag.transform.DOLocalMove(_highAnchor.localPosition, 3f);
        }

        public void RefreshFlagHeight(float damageTaken)
        {
            // float fromNormalized = (_rempart.health - damageTaken) / _rempart.maxHealth;
            Vector3 from = _flag.localPosition;
            Vector3 to = Vector3.Lerp(
                _lowAnchor.localPosition,
                _highAnchor.localPosition,
                _rempart.normalizedHealth
            );

            DOVirtual.Float(
                0f,
                1f,
                1f,
                (float value) =>
                {
                    _flag.localPosition = Vector3.Lerp(from, to, value);
                }
            );
        }

		#endregion

		#endregion

		#region Methods

		#endregion
    }
}
