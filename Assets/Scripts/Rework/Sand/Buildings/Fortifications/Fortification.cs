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

        protected int _quality = -1;

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
            if (_materialPropertyBlock != null)
            {
                _materialPropertyBlock.SetProperty(
                    MouldTool.SAND_CONCENTRATION_PROPERTY,
                    sandConcentration
                );
            }

            _quality = Mathf.FloorToInt(_fortificationChannel.qualityFromSandConcentration.Evaluate(
                sandConcentration
            ));
            

            _health = _maxHealth + ((float)_quality * _fortificationChannel.qualityCoef);

        }

		#endregion

		#endregion
    }
}
