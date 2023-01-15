namespace TideDefense
{
    using UnityEngine;

    public class Rempart : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private GameObject _visual = null;

        [SerializeField]
        private MeshFilter _visualMeshFilter = null;

        #endregion

        #region Methods

        public void SetRempartBlock(RempartBlock block)
        {
            _visualMeshFilter.mesh = block.mesh;
        }

        #endregion
    }
}
