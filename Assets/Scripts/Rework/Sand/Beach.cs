namespace TideDefense
{
    using UnityEngine;

    // TODO : Refact parameters of the Beach : BeachSlope, BeachBottom ...

    public class Beach : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

        #region Wetness

        [Header("Wetness")]
        [SerializeField]
        private WetnessSimulation _wetnessSimulation = null;

        [SerializeField]
        private LayerMask _beachLayerMask;

        private Ray ray;
        private RaycastHit hit;
        private Camera _camera = null;

        #endregion

		#endregion

        #region Methods

        #region MonoBehaviour

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, _beachLayerMask.value))
            {
                _gameplayChannel.onHoverBeach(hit);
            }
        }

        #endregion

        #region Wetness

        public void UpdateWetness(float[] beachCoveragePerSegment)
        {
            for (int i = 0; i < beachCoveragePerSegment.Length; i++)
                beachCoveragePerSegment[i] = beachCoveragePerSegment[i] / transform.localScale.z;

            _wetnessSimulation.RefreshTextureCoverage(beachCoveragePerSegment);
        }

        public float GetWetnessFromWorldPosition(Vector3 position)
        {
            float wetness = 0f;

            // Creates a ray above the beach, pointing down, for racast
            Ray ray = new Ray(position + Vector3.up, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2f, _beachLayerMask.value))
            {
                wetness = _wetnessSimulation.GetWetnessFromUVCoords(hit.textureCoord);
            }

            return wetness;
        }

        #endregion

        #region Raycast



        #endregion

        #endregion
    }
}
