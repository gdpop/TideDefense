namespace TideDefense
{
    using DG.Tweening;
    using UnityEngine;
    using VirtuoseReality.Rendering;

    public class FlagPole : MonoBehaviour
    {
		#region Fields

        private SandTower _rempart = null;
        public void Initialize(SandTower rempart)
        {
            _rempart = rempart;
        }

		#region Flag Color

        [Header("Color")]
        [SerializeField]
        private MaterialPropertyBlockModifier _materialPropertyBlock = null;

        private const string QUALITY_COLOR_PROPERTY = "_QualityColor";

        public void RefreshFlagColor(Color qualityColor)
        {
            _materialPropertyBlock.SetProperty(QUALITY_COLOR_PROPERTY, qualityColor);
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
