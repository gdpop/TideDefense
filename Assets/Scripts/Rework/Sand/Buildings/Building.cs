namespace TideDefense
{
    using UnityEngine;

    public class Building : MonoBehaviour
    {
		#region Health

        protected FortificationManager _fortificationManager = null;
        public FortificationManager fortificationManager
        {
            get { return _fortificationManager; }
            set { _fortificationManager = value; }
        }

        [SerializeField]
        private TilesetTypeFour _tilesetType = TilesetTypeFour.Empty;
        public TilesetTypeFour tilesetType
        {
            get { return _tilesetType; }
            set { _tilesetType = value; }
        }

        private GridCellModel _gridCellModel = null;
        public GridCellModel gridCellModel
        {
            get { return _gridCellModel; }
            set { _gridCellModel = value; }
        }

        protected float _health = 100f;
        public float health
        {
            get { return _health; }
        }

        protected float _maxHealth = 100f;
        public float maxHealth
        {
            get { return _maxHealth; }
        }

        public float normalizedHealth
        {
            get { return _health / _maxHealth; }
        }

        public virtual void Initialize(FortificationManager manager, GridCellModel cellModel)
        {
            _gridCellModel = cellModel;
            _gridCellModel.building = this;
            _fortificationManager = manager;
        }

        public virtual void InflictDamage(float damageTaken)
        {
            _health -= damageTaken;
            // Debug.Log($"Building {name} : health : {_health}");

            if (_health <= 0)
                _fortificationManager.DestroyBuilding(this);
        }

		#endregion

		#region Methods

		#endregion
    }
}
