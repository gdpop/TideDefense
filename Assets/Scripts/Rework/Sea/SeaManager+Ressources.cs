using CodesmithWorkshop.Useful;
using UnityEngine;

namespace TideDefense
{
    /// <summary>
    /// Everything the Sea bring to the player
    /// </summary>
    public partial class SeaManager
    {
		#region Fields

		#region Floating Objects

        private Bounds _floatingSpawnZone = new Bounds();

        [SerializeField]
        private FloatingObject _floatingPrefab = null;

        [SerializeField]
        private FloatingObjectSettings _floatingSettings = null;
        public FloatingObjectSettings floatingSettings
        {
            get { return _floatingSettings; }
        }

        [SerializeField]
        private Transform _floatingContainer = null;

        private Vector3 _submergedOffset;

		#endregion

		#endregion

		#region Methods

		#region Floating Objects

		#region MonoBehaviour

        private void Start_Ressources()
        {
            _submergedOffset = new Vector3(0, _floatingSettings.submergedOffsetY, 0);
        }

        private void Update_Ressources()
        {
            UpdateFloatingContainer();
            if (Input.GetKeyDown(KeyCode.K))
            {
                SpawnFloatingObject();
            }
        }

        private void OnDrawGizmos_Resources()
        {
            Gizmos.DrawWireCube(_floatingSpawnZone.center, _floatingSpawnZone.extents * 2f);
        }

		#endregion
        private void SpawnFloatingObject()
        {
            FloatingObject floating = Instantiate(
                _floatingPrefab,
                GetSpawnPosition(),
                Quaternion.identity,
                _floatingContainer
            );
            floating.Initialize(this);
        }

        private void UpdateFloatingContainer()
        {
            _floatingContainer.transform.position =
                _seaTransform.position + _floatingSettings.offsetFloatingContainer;
            _floatingSpawnZone.center = _floatingContainer.position;
            _floatingSpawnZone.extents = new Vector3(
                _floatingSettings.floatingSpawnZoneDimensions.x,
                0f,
                _floatingSettings.floatingSpawnZoneDimensions.y
            );
        }

        public Vector3 GetSpawnPosition()
        {
            return _floatingSpawnZone.RandomPosition() + _submergedOffset;
        }

		#endregion

		#endregion
    }
}
