namespace CodesmithWorkshop
{
    using System;
    using UnityEngine;
    using VirtuoseReality.Utils.TransformTools;
    using DG.Tweening;

    public class SphericalCameraController : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        protected SphericalCoordinatesTransform _sphericalTransform = null;

        [SerializeField]
        protected Transform _origin = null;

        [SerializeField]
        protected bool _isActive = true;
        protected bool isActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public SphericalCoordinates _startingCoordinates = new SphericalCoordinates();

        [Header("Rotation Settings")]
        [SerializeField]
        protected float _maxRotationInertia = 1f;

        [SerializeField]
        protected float _rotationAcceleration = 0.01f;

        [SerializeField]
        protected float _rotationDecelerationDuration = 0.01f;

        [SerializeField]
        protected float _minTheta = 20f;

        [SerializeField]
        protected float _maxTheta = 75f;

        protected Vector2Int _rotationInertia = new Vector2Int();
        protected Vector2 _currentRotationInertia = new Vector2();
        protected Tween _horizontalRotationFrictionTween = null;
        protected Tween _vertialRotationFrictionTween = null;
        protected float _currentTheta = 0f;

		#endregion

        #region Zoom settings

        // TODO Manage zoom on camera ?
        // [SerializeField]
        // protected float _maxZoomInertia = 1f;

        // [SerializeField]
        // protected float _zoomAcceleration = 0.01f;

        // [SerializeField]
        // protected float _minRadius = 20f;

        // [SerializeField]
        // protected float _maxRadius = 75f;

        // private float _currentZoomInertia = 0f;

        #endregion

		#region Methods

        #region MonoBehaviour

        protected virtual void Start()
        {
            _sphericalTransform.coordinates = _startingCoordinates;
        }

        protected virtual void OnEnable()
        {
            _horizontalRotationFrictionTween = DOVirtual.DelayedCall(0f, () => { });
            _horizontalRotationFrictionTween.Complete();
            _vertialRotationFrictionTween = DOVirtual.DelayedCall(0f, () => { });
            _vertialRotationFrictionTween.Complete();
        }

        protected virtual void Update()
        {
            // Computes Inertia on x axis (Phi angle)
            // ManageHorizontalMotion(Input.GetKey(KeyCode.Q), Input.GetKey(KeyCode.D));

            // Computes Inertia on y axis (Theta angle)
            // ManageVerticalMotion(Input.GetKey(KeyCode.Z), Input.GetKey(KeyCode.S));
            if (!_isActive)
                return;

            _sphericalTransform.coordinates.phi += _currentRotationInertia.x * _rotationInertia.x;

            _currentTheta = _sphericalTransform.coordinates.theta +=
                _currentRotationInertia.y * _rotationInertia.y;
            _currentTheta = Mathf.Clamp(_currentTheta, _minTheta, _maxTheta);

            _sphericalTransform.coordinates.theta = _currentTheta;
        }

        protected virtual void LateUpdate()
        {
            if (!_isActive)
                return;

            _sphericalTransform.transform.LookAt(_origin);
        }

        #endregion


        protected void ManageHorizontalMotion(bool isLeft, bool isRight)
        {
            if (isLeft)
            {
                if (_horizontalRotationFrictionTween != null)
                    _horizontalRotationFrictionTween.Kill();

                if (_rotationInertia.x == 1)
                    _currentRotationInertia.x = 0;

                _rotationInertia.x = -1;
                _currentRotationInertia.x += _rotationAcceleration;
                _currentRotationInertia.x = Mathf.Clamp(
                    _currentRotationInertia.x,
                    0f,
                    _maxRotationInertia
                );
            }
            else if (isRight)
            {
                if (_horizontalRotationFrictionTween != null)
                    _horizontalRotationFrictionTween.Kill();

                if (_rotationInertia.x == -1)
                    _currentRotationInertia.x = 0;

                _rotationInertia.x = 1;
                _currentRotationInertia.x += _rotationAcceleration;
                _currentRotationInertia.x = Mathf.Clamp(
                    _currentRotationInertia.x,
                    0f,
                    _maxRotationInertia
                );
            }
            else if (_currentRotationInertia.x > 0 && !_horizontalRotationFrictionTween.active)
            {
                _horizontalRotationFrictionTween = DOVirtual.Float(
                    _currentRotationInertia.x,
                    0f,
                    _rotationDecelerationDuration,
                    (float value) =>
                    {
                        _currentRotationInertia.x = value;
                    }
                );
            }
        }

        protected void ManageVerticalMotion(bool isUp, bool isDown)
        {
            if (isUp)
            {
                if (_vertialRotationFrictionTween != null)
                    _vertialRotationFrictionTween.Kill();

                if (_rotationInertia.y == -1)
                    _currentRotationInertia.y = 0;

                _rotationInertia.y = 1;
                _currentRotationInertia.y += _rotationAcceleration;
                _currentRotationInertia.y = Mathf.Clamp(
                    _currentRotationInertia.y,
                    0f,
                    _maxRotationInertia
                );
            }
            else if (isDown)
            {
                if (_vertialRotationFrictionTween != null)
                    _vertialRotationFrictionTween.Kill();

                if (_rotationInertia.y == 1)
                    _currentRotationInertia.y = 0;

                _rotationInertia.y = -1;
                _currentRotationInertia.y += _rotationAcceleration;
                _currentRotationInertia.y = Mathf.Clamp(
                    _currentRotationInertia.y,
                    0f,
                    _maxRotationInertia
                );
            }
            else if (_currentRotationInertia.y > 0 && !_vertialRotationFrictionTween.active)
            {
                _vertialRotationFrictionTween = DOVirtual.Float(
                    _currentRotationInertia.y,
                    0f,
                    _rotationDecelerationDuration,
                    (float value) =>
                    {
                        _currentRotationInertia.y = value;
                    }
                );
            }
        }




		#endregion
    }
}
