namespace TideDefense
{
	using UnityEngine;
	
	public class Building : MonoBehaviour {


		#region Health

		protected FortificationManager _fortificationManager = null;
        public FortificationManager fortificationManager
        {
            get { return _fortificationManager; }
            set { _fortificationManager = value; }
        }

		private GridCellModel _gridCell = null;
        public GridCellModel gridCell
        {
            get { return _gridCell; }
            set { _gridCell = value; }
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
            _gridCell = gridCell;
            _fortificationManager = manager;
        }

		public virtual void InflictDamage(float damageTaken)
        {
            _health -= damageTaken;
            Debug.Log($"Building {name} : health : {_health}");

            if (_health <= 0)
                _fortificationManager.DestroyBuilding(this);
        }
		
		#endregion 
		
		#region Methods 
		
		#endregion
	}
}