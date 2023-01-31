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

        private SeaManager _seaManager = null;
        private float _timeOffset = 0f;
        private Vector3 _startPosition = new Vector3();
        private Vector3 _computedPosition = new Vector3();

		#region Apparition Movement

        private Vector3 _apparitionPosition = new Vector3();

		#endregion

		#region Forward Movement

        private Vector3 _forwardPosition = new Vector3();

		#endregion

		#region Up & Down Movement

        private Vector3 _upAndDownPosition = new Vector3();
        private float _upAndDownSpeed = 0f;
        private float _upAndDownValue = 0f;
        private float _upAndDownTime = 0;


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



        public void Initialize(SeaManager seaManager)
        {
            _startPosition = transform.localPosition;
			_seaManager = seaManager;
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
            _forwardPosition.z += deltaTime * _seaManager.floatingSettings.forwardSpeed;
        }

		#endregion

		#region Up & Down

        public void ManageUpAndDown(float deltaTime)
        {
            _upAndDownTime += deltaTime * _upAndDownSpeed;
            _upAndDownValue = Mathf.Sin(_upAndDownTime) * _seaManager.floatingSettings._upAndDownAmplitude;

            _upAndDownPosition.y = _upAndDownValue;
        }

		#endregion

		#endregion
    }
}
