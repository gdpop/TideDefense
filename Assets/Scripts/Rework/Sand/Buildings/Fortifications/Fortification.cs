namespace TideDefense
{
    using UnityEngine;
    using VirtuoseReality.Rendering;

    public class Fortification : Building
    {
		#region Fields

        [SerializeField]
        protected FortificationChannel _fortificationChannel = null;

        [SerializeField]
        protected MaterialPropertyBlockModifier _materialPropertyBlock = null;

        protected float _quality = 0f;

		#endregion

		#region Methods

        public virtual void Initialize(
            FortificationManager manager,
            GridCellModel cellModel,
            float sandConcentration
        )
        {
            Initialize(manager, cellModel);
            SetHealthFromSandConcentration(sandConcentration);
        }

		#region Quality Sand Concentration

        protected virtual void SetHealthFromSandConcentration(float sandConcentration)
        {
            _materialPropertyBlock.SetFloat(
                MouldTool.SAND_CONCENTRATION_PROPERTY,
                sandConcentration
            );
            _quality = _fortificationChannel.qualityFromSandConcentration.Evaluate(
                sandConcentration
            );

            _health = _maxHealth + (_quality * _fortificationChannel.qualityCoef);
        }
		#endregion

		#endregion
    }
}
