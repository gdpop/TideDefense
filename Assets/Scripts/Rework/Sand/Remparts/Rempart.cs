namespace TideDefense
{
    using System;
    using UnityEngine;
    using VirtuoseReality.Rendering;

    // TODO : Rename Rempart into SandTower


    // TODO : Dissociate Normalized Health & Actual Health
    // TODO : Castle Quality(Max Health) is the color of the flag
    // TODO : Castle Normalized Health is the height of the flag
    public class Rempart : MonoBehaviour
    {
        #region Fields

        private RempartsManager _rempartsManager = null;
        public RempartsManager rempartsManager
        {
            get { return _rempartsManager; }
            set { _rempartsManager = value; }
        }

        private GridCellModel _gridCell = null;
        public GridCellModel gridCell
        {
            get { return _gridCell; }
            set { _gridCell = value; }
        }

        [SerializeField]
        private MaterialPropertyBlockModifier _materialPropertyBlock = null;

        #region Health

        private float _health = 100f;
        public float health
        {
            get { return _health; }
        }

        private float _maxHealth = 100f;
        public float maxHealth
        {
            get { return _maxHealth; }
        }

        public float normalizedHealth
        {
            get { return _health / _maxHealth; }
        }

        #endregion

        #region Flag Pole

        [SerializeField]
        private FlagPole _flagPole = null;

        #endregion

        #endregion

        #region Methods

        [Obsolete]
        public void SetRempartBlock(RempartBlock block)
        {
            // _visualMeshFilter.mesh = block.mesh;
        }

        private void Awake() {
            _flagPole.Initialize(this);
            
        }

        public void Initialize(float sandConcentration)
        {
            SetHealthFromSandConcentration(sandConcentration);
            _flagPole.RaiseFlag();
        }

        #region Health

        [SerializeField]
        private AnimationCurve _qualityFromSandConcentration = null;

        private float _quality = 0f;

        [SerializeField]
        private float _qualityCoef = 100f;

        public void InflictDamage(float damageTaken)
        {
            _flagPole.RefreshFlagHeight(damageTaken);
            _health -= damageTaken;
            Debug.Log($"Rempart {name} : health : {_health}");

            if (_health <= 0)
                _rempartsManager.DestroyRempartReworked(this);
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
