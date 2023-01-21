namespace TideDefense
{
    using UnityEngine;

    // TODO : Refact parameters of the Beach : BeachSlope, BeachBottom ...

    public class Beach : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private GameplayChannel _gameplayChannel = null;

        [SerializeField]
        private SeaChannel _seaChannel = null;

        #region Wetness

        [Header("Wetness")]
        [SerializeField]
        private WetnessSimulation _wetnessSimulation = null;

        [SerializeField]
        private LayerMask _beachLayerMask;

        #endregion

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
    }
}
