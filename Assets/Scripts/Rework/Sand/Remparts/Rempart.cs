namespace TideDefense
{
    using UnityEngine;

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
        private GameObject _visual = null;

        [SerializeField]
        private MeshFilter _visualMeshFilter = null;

        #region Health

        [SerializeField]
        private float _health = 1f;
        public float health
        {
            get { return _health; }
            set { _health = value; }
        }

        #endregion

        #endregion

        #region Methods

        public void SetRempartBlock(RempartBlock block)
        {
            _visualMeshFilter.mesh = block.mesh;
        }

        public void InflictDamage(float damageInflicted)
        {
            _health -= damageInflicted;
            Debug.Log($"Rempart {name} : health : {_health}");

            if(_health <= 0)
                _rempartsManager.DestroyRempart(this);
        }

        #endregion
    }
}
