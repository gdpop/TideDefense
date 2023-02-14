using System.Collections.Generic;
using System.Collections;
using CodesmithWorkshop.Useful;
using UnityEngine;
using System.Linq;

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
        private SequencerChannel _sequencerChannel = null;

        [SerializeField]
        private Animator _sequencerController = null;

        [SerializeField]
        private RuntimeAnimatorController _tutorialPlayable;

        [SerializeField]
        private RuntimeAnimatorController _withoutTutorialPlayable;

        private const string START_SEQUENCER = "StartSequencer";

        [Header("Floating Objects")]
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

        private List<FloatingObject> _floatingObjects = new List<FloatingObject>();

        private Bounds _floatingSpawnZone = new Bounds();

        private Vector3 _submergedOffset;

        /// <summary>
        /// When floatingObject reach this possition in the sea, the stop moving and are about to be wahed-up on the beach
        /// </summary>
        public Vector3 limitBeforeWashUp
        {
            get { return _currentTidePosition - _floatingSettings.washUpOffset; }
        }

        private IEnumerator _spawnNarrationMessageCoroutine = null;

		#endregion

        #region Message Bottle

        [SerializeField]
        private MessageBottleSettings _messageBottleSettings = null;

        private List<MessageBottleData> _narrationMessageBottleDatas =
            new List<MessageBottleData>();

        private float _currentDelayMessageNarration = 0;

        #endregion

        #region Washed Up Object

        [Header("Wash Up")]
        [SerializeField]
        private Transform _washedUpContainer = null;
        public Transform washedUpContainer
        {
            get { return _washedUpContainer; }
        }

        [SerializeField]
        private float _washedUpObjectBeachCoverageOffset = 2f;

        [SerializeField]
        private float _washedUpObjectRandomRange = 1;

        #endregion

		#endregion

		#region Methods

        #region MonoBehaviour

        private void Start_Ressources()
        {
            _narrationMessageBottleDatas = _messageBottleSettings.narrationMessageBottleDatas;

            _submergedOffset = new Vector3(0, _floatingSettings.submergedOffsetY, 0);

            if (_sequencerChannel != null)
            {
                _sequencerChannel.onCreateFloatingObject += CallbackCreateFloatingObject;
                _sequencerChannel.onCreatedWashedUpObject += CallbackCreateWashedUpObject;

                _sequencerChannel.onCreateFloatingMessageBottle +=
                    CallbackCreateFloatingMessageBottle;
                _sequencerChannel.onCreateWashedUpMessageBottle +=
                    CallbackCreateWashedUpMessageBottle;
            }

            StartSpawningNarrationMessage();
        }

        private void Update_Ressources()
        {
            UpdateFloatingContainer();
        }

        private void OnDrawGizmos_Resources()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(limitBeforeWashUp, limitBeforeWashUp + new Vector3(2, 0, 0));
            Gizmos.DrawWireCube(_floatingSpawnZone.center, _floatingSpawnZone.extents * 2f);
        }

        private void OnDestroy_FloatingSequencer()
        {
            if (_sequencerChannel != null)
            {
                _sequencerChannel.onCreateFloatingObject -= CallbackCreateFloatingObject;
                _sequencerChannel.onCreatedWashedUpObject -= CallbackCreateWashedUpObject;
                _sequencerChannel.onCreateFloatingMessageBottle -=
                    CallbackCreateFloatingMessageBottle;
                _sequencerChannel.onCreateWashedUpMessageBottle -=
                    CallbackCreateWashedUpMessageBottle;
            }
        }

		#endregion

        #region Behaviour

        public void PlayTutorial()
        {
            _sequencerController.runtimeAnimatorController = _tutorialPlayable;
            _sequencerController.SetTrigger(START_SEQUENCER);
        }

        public void PlayWithoutTutorial()
        {
            _sequencerController.runtimeAnimatorController = _withoutTutorialPlayable;
            _sequencerController.SetTrigger(START_SEQUENCER);
        }

        #endregion

		#region Floating Objects

        public void CallbackCreateFloatingObject(FloatingObject floatingObject)
        {
            FloatingObject floating = Instantiate(
                floatingObject,
                GetFloatingRandomPosition(),
                Quaternion.identity,
                _floatingContainer
            );

            _floatingObjects.Add(floating);
            floating.Initialize(this);
        }

        #region Floating Sequencer

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

        public Vector3 GetFloatingRandomPosition()
        {
            return _floatingSpawnZone.RandomPosition() + _submergedOffset;
        }

        #endregion

        #region Message Bottle

        public void CreateFloatingMessageBottle(MessageBottleData data)
        {
            FloatingObject floating = Instantiate(
                _floatingPrefab,
                GetFloatingRandomPosition(),
                Quaternion.identity,
                _floatingContainer
            );

            MessageBottle message = Instantiate(
                _messageBottleSettings.PrefabFromType(data.type),
                Vector3.zero,
                Quaternion.identity,
                floating.objectContainer
            );

            message.Initialize(data);
            message.transform.localPosition = Vector3.zero;

            floating.onWashUpComplete.AddListener(message.SetWashedUp);
            _floatingObjects.Add(floating);
            floating.Initialize(this);
        }

        public void CallbackCreateFloatingMessageBottle(MessageBottleData data)
        {
            CreateFloatingMessageBottle(data);
        }

        public void CallbackCreateWashedUpMessageBottle(MessageBottleData data)
        {
            MessageBottle bottle = Instantiate(
                _messageBottleSettings.PrefabFromType(data.type),
                GetRandomWashedUpPosition(),
                UtilsClass.RandomRotation(),
                _washedUpContainer
            );
            bottle.Initialize(data);
            bottle.SetWashedUp();
        }

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
                _currentDelayMessageNarration = UnityEngine.Random.Range(
                    _messageBottleSettings.minDelayMessageNarration,
                    _messageBottleSettings.maxDelayMessageNarration
                );

                yield return new WaitForSeconds(_currentDelayMessageNarration);

                CreateMessageNarration();

                yield return null;
            }
        }

        private void CreateMessageNarration()
        {
            Debug.Log(_narrationMessageBottleDatas.Count);
            MessageBottleData data =
                _messageBottleSettings.narrationMessageBottleDatas.PickRandom<MessageBottleData>();
            CreateFloatingMessageBottle(data);
            _narrationMessageBottleDatas.Remove(data);
            Debug.Log("Après" + _narrationMessageBottleDatas.Count);
        }

        #endregion

        #endregion

        #region Wash Up

        public void WashUpFloatingObject()
        {
            foreach (FloatingObject floating in _floatingObjects)
            {
                if (floating.state == FloatingObjectState.Waiting)
                {
                    WaveSegment segment = FindClosestWaveSegment(floating, _currentWave);
                    Vector3 toPosition = GetPositionFromBeachCoverage(
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

        #region Washed Up Objects

        [SerializeField]
        private WashedUpObject testObject = null;

        [ContextMenu("TestWashedUp")]
        public void TestWashedUp()
        {
            CallbackCreateWashedUpObject(testObject);
        }

        private void CallbackCreateWashedUpObject(WashedUpObject washedUpObject)
        {
            Vector3 rndPosition = GetRandomWashedUpPosition();
            WashedUpObject newWashedUp = Instantiate(
                washedUpObject,
                rndPosition,
                Quaternion.identity,
                _washedUpContainer
            );
            newWashedUp.Initialize();
        }

        private Vector3 GetRandomWashedUpPosition()
        {
            Vector3 randomPosition = GetPositionFromBeachCoverage(
                _tideBeachCoverage + _washedUpObjectBeachCoverageOffset
            );
            randomPosition.x =
                _beach.transform.position.x
                + UnityEngine.Random.Range(-_washedUpObjectRandomRange, _washedUpObjectRandomRange);
            return randomPosition;
        }

        #endregion

		#endregion
    }
}
