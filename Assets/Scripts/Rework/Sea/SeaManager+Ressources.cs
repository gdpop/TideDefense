using System.Collections.Generic;
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

        [Header("Floating Objects")]
        [SerializeField]
        private FloatingObject _floatingPrefab = null;

        [SerializeField]
        private FloatingObjectSettings _floatingSettings = null;
        public FloatingObjectSettings floatingSettings
        {
            get { return _floatingSettings; }
        }

        public Vector3 washUpLimit
        {
            get { return _currentTidePosition - _floatingSettings.washUpOffset; }
        }

        [SerializeField]
        private Transform _floatingContainer = null;

        private List<FloatingObject> _floatingObjects = new List<FloatingObject>();

        private Bounds _floatingSpawnZone = new Bounds();

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
            Gizmos.color = Color.red;
            Gizmos.DrawLine(washUpLimit, washUpLimit + new Vector3(2, 0, 0));
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
            _floatingObjects.Add(floating);
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

        #region Wash

        public void WashUpFloatingObject()
        {
            foreach (FloatingObject floating in _floatingObjects)
            {
                if (floating.state == FloatingObjectState.Waiting)
                {
                    WaveSegment segment = FindClosestWaveSegment(floating, _currentWave);
                    Vector3 toPosition = PositionFromBeachCoverave(
                        _tideBeachCoverage
                            + segment.totalBeachCoverage * _floatingSettings.apexBeachCoveragePercent
                    );

                    floating.WashUp(toPosition, segment.totalDelay);
                }
            }
        }

        public WaveSegment FindClosestWaveSegment(FloatingObject floating, Wave wave)
        {
            WaveSegment segment = null;
            float distance = 0;
            float lastDistance = 999;

            int count = wave.waveSegments.Count;
            for (int i = 0; i < count; i++)
            {
                segment = wave.waveSegments[i];
                distance = Vector3.Distance(
                    floating.transform.position,
                    segment.transform.position
                );

                if (lastDistance < distance)
                    return wave.waveSegments[i - 1];
                else
                    lastDistance = distance;
            }

            return segment;
        }

        #endregion

		#endregion

		#endregion
    }
}
