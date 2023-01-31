namespace TideDefense
{
    using CodesmithWorkshop.Useful;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.Events;

    // Apparition
    // Progression
    //

    public class FloatingObject : MonoBehaviour
    {
		#region Fields

        public TimeChannel _timeChannel = null;
        
        private SeaManager _seaManager = null;
        private float _timeOffset = 0f;
        private Vector3 _startPosition = new Vector3();

        [SerializeField]
        private FloatingObjectState _state = FloatingObjectState.None;
        public FloatingObjectState state
        {
            get { return _state; }
            set { _state = value; }
        }

		#region Apparition Movement

        private Vector3 _apparitionPosition = new Vector3();

		#endregion

		#region Forward Movement

        private Vector3 _forwardPosition = new Vector3();

        private float _forwardSpeed = 0f;

		#endregion

		#region Up & Down Movement

        private Vector3 _upAndDownPosition = new Vector3();
        private float _upAndDownSpeed = 0f;
        private float _upAndDownValue = 0f;
        private float _upAndDownTime = 0;

		#endregion

        #region Wash Up

        public UnityEvent _onWashedUp = new UnityEvent();

        #endregion

		#endregion

		#region Methods

		#region MonoBehaviour

        private void Start()
        {
            _timeOffset = Random.Range(0f, 100f);

            if (_timeChannel != null)
            {
                _timeChannel.onUpdateCurrentDeltaTime += CallbackUpdateCurrentDeltaTime;
            }
        }

        private void Update()
        {
            if (_state != FloatingObjectState.WashingUp)
                UpdateMovement();
                
        }

        private void LateUpdate()
        {
            if (
                _state == FloatingObjectState.Floating
                && transform.position.z >= _seaManager.washUpLimit.z
            )
            {
                StopForwardMovement();
            }
        }

        private void OnDestroy()
        {
            if (_timeChannel != null)
            {
                _timeChannel.onUpdateCurrentDeltaTime -= CallbackUpdateCurrentDeltaTime;
            }
        }

		#endregion

		#region Behaviour

        public void Initialize(SeaManager seaManager)
        {
            _startPosition = transform.localPosition;
            _seaManager = seaManager;
            _forwardSpeed = _seaManager.floatingSettings.forwardSpeed;
            InitializeApparition();
            _state = FloatingObjectState.Floating;
        }

        public void UpdateMovement()
        {
            transform.localPosition =
                _startPosition + (_apparitionPosition + _upAndDownPosition + _forwardPosition);
        }

        public void StopForwardMovement()
        {
            _forwardSpeed = 0f;
            _state = FloatingObjectState.Waiting;
        }

        public void CallbackUpdateCurrentDeltaTime(float deltaTime)
        {
            ManageForward(deltaTime);
            ManageUpAndDown(deltaTime);
        }

		#endregion

		#region Apparition



        public void InitializeApparition()
        {
            _upAndDownSpeed = 0f;

            DOVirtual
                .Float(
                    0f,
                    -_seaManager.floatingSettings.submergedOffsetY,
                    _seaManager.floatingSettings.apparitionDuration,
                    (float value) =>
                    {
                        _apparitionPosition.y = value;
                    }
                )
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    _upAndDownSpeed = _seaManager.floatingSettings.upAndDownSpeed;
                });
        }

		#endregion

		#region Forward

        public void ManageForward(float deltaTime)
        {
            _forwardPosition.z += deltaTime * _forwardSpeed;
        }

		#endregion

		#region Up & Down

        public void ManageUpAndDown(float deltaTime)
        {
            _upAndDownTime += deltaTime * _upAndDownSpeed;
            _upAndDownValue =
                Mathf.Sin(_upAndDownTime) * _seaManager.floatingSettings.upAndDownAmplitude;

            _upAndDownPosition.y = _upAndDownValue;
        }

		#endregion

        #region WashUp

        public void WashUp(Vector3 position, float totalDelay)
        {
            _state = FloatingObjectState.WashingUp;
            _upAndDownSpeed = 0;
            position.x = transform.position.x;

            transform
                .DOMove(position, _seaManager.floatingSettings.washUpDuration)
                .SetEase(_seaManager.floatingSettings.washUpEase)
                .SetDelay(totalDelay)
                .OnComplete(() =>
                {
                    _onWashedUp.Invoke();
                });
        }

        #endregion

		#endregion
    }
}
