namespace TideDefense
{
    using CodesmithWorkshop.Useful;
    using DG.Tweening;
    using UnityEngine;

    // Apparition
    // Progression
    //

    public class FloatingObject : MonoBehaviour
    {
		#region Fields

        public TimeChannel _timeChannel = null;

        [SerializeField]
        private Sandbox _seaManager = null;

        private float _timeOffset = 0f;

        private Vector3 _startPosition = new Vector3();

        private Vector3 _computedPosition = new Vector3();

		#region Apparition Movement

        [Header("Apparition")]
        [SerializeField]
        private float _apparitionDuration = 1f;

        private Vector3 _apparitionPosition = new Vector3();

		#endregion

		#region Forward Movement

        [Header("Forward")]
        [SerializeField]
        private float _forwardSpeed = 0.1f;

        private Vector3 _forwardPosition = new Vector3();

		#endregion

		#region Up & Down Movement

        [Header("Up & Down")]
        [SerializeField]
        private float _upAndDownSpeed = 1f;
        private float _upAndDownSpeedSaved = 0f;

        [SerializeField]
        private float _upAndDownAmplitude = 1f;

        private float _upAndDownValue = 0f;

        private float _upAndDownTime = 0;

        private Vector3 _upAndDownPosition = new Vector3();

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

            InitializeMovement();
        }

        private void Update()
        {
            UpdateMovement();
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



        public void InitializeMovement()
        {
            _startPosition = _seaManager.GetSpawnPosition();
            transform.localPosition = _startPosition;
            InitializeApparition();
        }

        public void UpdateMovement()
        {
            transform.localPosition =
                _startPosition + (_apparitionPosition + _upAndDownPosition + _forwardPosition);
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
            _upAndDownSpeedSaved = _upAndDownSpeed;
            _upAndDownSpeed = 0f;

            DOVirtual
                .Float(
                    0f,
                    -_seaManager.submergedOffsetY,
                    _apparitionDuration,
                    (float value) =>
                    {
                        _apparitionPosition.y = value;
                    }
                )
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    _upAndDownSpeed = _upAndDownSpeedSaved;
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
            _upAndDownValue = Mathf.Sin(_upAndDownTime) * _upAndDownAmplitude;
            // _upAndDownValue = UtilsClass.MinusPlusToZeroPlus(_upAndDownValue);

            _upAndDownPosition.y = _upAndDownValue;
        }

		#endregion

		#endregion
    }
}
