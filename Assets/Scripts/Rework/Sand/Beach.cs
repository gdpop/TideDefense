namespace TideDefense
{
    using System.Collections.Generic;
    using PierreMizzi.MouseInteractable;
    using UnityEngine;

    // TODO : Refact parameters of the Beach : BeachSlope, BeachBottom ...
    // TODO : Separate visual and logical elements

    public class Beach : MonoBehaviour, IClickable, IHoverable
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

        #endregion

		#endregion

		#region IInteractable

        [SerializeField]
        private bool _isInteractable = true;
        public bool isInteractable
        {
            get { return _isInteractable; }
            set { _isInteractable = value; }
        }

		#endregion

		#region IClickable

        private bool _isClickable = true;
        public bool isClickable
        {
            get { return _isClickable; }
            set { _isClickable = value; }
        }

        public void OnLeftClick(RaycastHit hit)
        {
            _gameplayChannel.onClickBeach.Invoke(hit);
        }

		#endregion

        #region IHoverable

        private bool _isHoverable = true;
        public bool isHoverable
        {
            get { return _isHoverable; }
            set { _isHoverable = value; }
        }

        [SerializeField]
        private bool _isHovered = false;
        public bool isHovered
        {
            get { return _isHovered; }
            set { _isHovered = value; }
        }

        public void OnHoverEnter(RaycastHit hit)
        {
            isHovered = true;
        }

        public void OnHoverExit()
        {
            isHovered = false;
        }

        public void OnHover(RaycastHit hit)
        {
            _gameplayChannel.onHoverBeach.Invoke(hit);
        }

        #endregion

        #region Wetness

        public void UpdateWetness(float[] beachCoveragePerSegment)
        {
            for (int i = 0; i < beachCoveragePerSegment.Length; i++)
                beachCoveragePerSegment[i] = beachCoveragePerSegment[i] / transform.localScale.z;

            _wetnessSimulation.RefreshTextureCoverage(beachCoveragePerSegment);
        }

        [SerializeField]
        private LayerMask _beachLayerMask;

        public float GetWetnessFromWorldPosition(Vector3 position)
        {
            float wetness = 0f;

            // Creates a ray above the beach, pointing down, for racast
            Ray ray = new Ray(position + Vector3.up, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _beachLayerMask.value))
            {
                Debug.Log($"We raycasted with {hit.collider.name}");
                wetness = _wetnessSimulation.GetWetnessFromUVCoords(hit.textureCoord);
                Debug.Log($"Wetness : {wetness}");
            }

            return wetness;
        }

        #endregion
    }
}
