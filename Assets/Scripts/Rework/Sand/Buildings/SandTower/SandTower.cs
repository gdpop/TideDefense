namespace TideDefense
{
    using UnityEngine;
    using VirtuoseReality.Rendering;

    // TODO : Dissociate Normalized Health & Actual Health
    public class SandTower : Building
    {
        #region Fields

        [SerializeField]
        private MaterialPropertyBlockModifier _materialPropertyBlock = null;

        #region Flag Pole

        [SerializeField]
        private FlagPole _flagPole = null;

        #endregion

        #endregion

        #region Methods

        private void Awake() {
            _flagPole.Initialize(this);
        }

        public void Initialize(FortificationManager manager, GridCellModel cellModel, float sandConcentration)
        {
            Initialize(manager, gridCell);
            SetHealthFromSandConcentration(sandConcentration);
        }

        #region Health

        [SerializeField]
        private AnimationCurve _qualityFromSandConcentration = null;

        private float _quality = 0f;

        [SerializeField]
        private float _qualityCoef = 100f;

		public override void InflictDamage(float damageTaken)
        {
            _flagPole.RefreshFlagHeight(damageTaken);
            base.InflictDamage(damageTaken);
        }


        public void SetHealthFromSandConcentration(float sandConcentration)
        {
            _materialPropertyBlock.SetFloat(
                Bucket.SAND_CONCENTRATION_PROPERTY,
                sandConcentration
            );
            _quality = _qualityFromSandConcentration.Evaluate(sandConcentration);

            _health = _maxHealth + (_quality * _qualityCoef);
            _flagPole.RefreshFlagColor(_quality);
        }

        #endregion

        #endregion
    }
}
