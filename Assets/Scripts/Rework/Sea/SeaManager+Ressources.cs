using System.Collections.Generic;
using System.Collections;
using CodesmithWorkshop.Useful;
using DG.Tweening;
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

        [Header("Sequencer")]
        [SerializeField]
        private FloatingSequencerChannel _sequencerChannel = null;

        [SerializeField]
        private Animator _sequencerController = null;

        private const string START_SEQUENCER = "StartSequencer";

        [Header("Floating Objects")]
        [SerializeField]
        private FloatingObject _floatingPrefab = null;

        [SerializeField]
        private FloatingMessageBottle _floatingMessageBottle = null;

        [SerializeField]
        private FloatingObjectSettings _floatingSettings = null;
        public FloatingObjectSettings floatingSettings
        {
            get { return _floatingSettings; }
        }

        [SerializeField]
        private Transform _floatingContainer = null;

        private List<FloatingObject> _floatingObjects = new List<FloatingObject>();

        private Bounds _floatingSpawnZone = new Bounds();

        private Vector3 _submergedOffset;

        [Header("Narration")]
        [SerializeField]
        private float _minDelayMessageNarration = 20;

        [SerializeField]
        private float _maxDelayMessageNarration = 40;
        private float _currentDelayMessageNarration = 0;

        [SerializeField]
        private List<MessageBottleData> _narrationMessageBottleDatas =
            new List<MessageBottleData>();

        private IEnumerator _spawnNarrationMessageCoroutine = null;

        [Header("Wash Up")]
        [SerializeField]
        private Transform _washedUpContainer = null;
        public Transform washedUpContainer
        {
            get { return _washedUpContainer; }
        }

        public Vector3 washUpLimit
        {
            get { return _currentTidePosition - _floatingSettings.washUpOffset; }
        }

		#endregion

		#endregion

		#region Methods

        #region MonoBehaviour

        private void Start_Ressources()
        {
            _submergedOffset = new Vector3(0, _floatingSettings.submergedOffsetY, 0);

            if (_sequencerChannel != null)
            {
                _sequencerChannel.onCreateBeachTool += CallbackCreateBeachTool;
                _sequencerChannel.onCreateMessageBottle += CallbackCreateMessageBottle;
            }

            _sequencerController.SetTrigger(START_SEQUENCER);
            StartSpawningNarrationMessage();
        }

        private void Update_Ressources()
        {
            UpdateFloatingContainer();
        }

        private void OnDrawGizmos_Resources()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(washUpLimit, washUpLimit + new Vector3(2, 0, 0));
            Gizmos.DrawWireCube(_floatingSpawnZone.center, _floatingSpawnZone.extents * 2f);
        }

        private void OnDestroy_FloatingSequencer()
        {
            if (_sequencerChannel != null)
            {
                _sequencerChannel.onCreateBeachTool -= CallbackCreateBeachTool;
                _sequencerChannel.onCreateMessageBottle -= CallbackCreateMessageBottle;
            }
        }

		#endregion


		#region Floating Objects

        public void CreateMessageBottle(MessageBottleData data)
        {
            FloatingMessageBottle floating = Instantiate(
                _floatingMessageBottle,
                GetSpawnPosition(),
                Quaternion.identity,
                _floatingContainer
            );
            _floatingObjects.Add(floating);
            floating.Initialize(this, data);
        }

        #region Floating Sequencer

        public void CallbackCreateMessageBottle(MessageBottleData data)
        {
            CreateMessageBottle(data);
        }

        public void CallbackCreateBeachTool(BeachTool tool) { }

        public void DestroyFloatingObject(FloatingObject floating)
        {
            if (_floatingObjects.Contains(floating))
                _floatingObjects.Remove(floating);

            Destroy(floating.gameObject);
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

        #region Narration

        private void StartSpawningNarrationMessage()
        {
            if (_spawnNarrationMessageCoroutine == null)
            {
                _spawnNarrationMessageCoroutine = SpawnNarrationMessage();
                StartCoroutine(_spawnNarrationMessageCoroutine);
            }
        }

        private void StopSpawningNarrationMessage()
        {
            if (_spawnNarrationMessageCoroutine != null)
            {
                StopCoroutine(_spawnNarrationMessageCoroutine);
                _spawnNarrationMessageCoroutine = null;
            }
        }

        public IEnumerator SpawnNarrationMessage()
        {
            while (true)
            {
                _currentDelayMessageNarration = Random.Range(
                    _minDelayMessageNarration,
                    _maxDelayMessageNarration
                );

                yield return new WaitForSeconds(_currentDelayMessageNarration);

                CreateMessageNarration();

                yield return null;
            }
        }

        private void CreateMessageNarration()
        {
            MessageBottleData data = _narrationMessageBottleDatas.PickRandom<MessageBottleData>();
            CreateMessageBottle(data);
            _narrationMessageBottleDatas.Remove(data);
        }

        #endregion

        #region Wash Up

        public void WashUpFloatingObject()
        {
            foreach (FloatingObject floating in _floatingObjects)
            {
                if (floating.state == FloatingObjectState.Waiting)
                {
                    WaveSegment segment = FindClosestWaveSegment(floating, _currentWave);
                    Vector3 toPosition = PositionFromBeachCoverave(
                        _tideBeachCoverage
                            + segment.totalBeachCoverage
                                * _floatingSettings.apexBeachCoveragePercent
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
