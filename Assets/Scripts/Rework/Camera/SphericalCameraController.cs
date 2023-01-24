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
        protected bool _isActive = true;
        protected bool isActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        [SerializeField]
        protected Transform _origin = null;

        [Header("Controls values")]
        [SerializeField]
        protected Vector2Int _currentDirection = new Vector2Int();

        [SerializeField]
        protected Vector2 _currentInertia = new Vector2();

        [SerializeField]
        protected float _maxInertia = 1f;

        [SerializeField]
        protected float _acceleration = 0.01f;

        [SerializeField]
        protected float _friction = 0.01f;

        protected Tween _horizontalFrictionTween = null;
        protected Tween _vertialFrictionTween = null;

		#region Theta

        [Header("Theta")]
        [SerializeField]
        protected float _startTheta = 33f;

        [SerializeField]
        protected float _minTheta = 20f;

        [SerializeField]
        protected float _maxTheta = 75f;

        protected float _currentTheta = 0f;

		#endregion

        [SerializeField]
        protected SphericalCoordinatesTransform _sphericalTransform = null;

		#endregion

		#region Methods

        protected virtual void OnEnable()
        {
            _horizontalFrictionTween = DOVirtual.DelayedCall(0f, () => { });
            _horizontalFrictionTween.Complete();
            _vertialFrictionTween = DOVirtual.DelayedCall(0f, () => { });
            _vertialFrictionTween.Complete();

            _sphericalTransform.theta = _startTheta;
        }

        protected virtual void Update()
        {
            // Computes Inertia on x axis (Phi angle)
            // ManageHorizontalMotion(Input.GetKey(KeyCode.Q), Input.GetKey(KeyCode.D));

            // Computes Inertia on y axis (Theta angle)
            // ManageVerticalMotion(Input.GetKey(KeyCode.Z), Input.GetKey(KeyCode.S));
            if (!_isActive)
                return;

            _sphericalTransform.phi += _currentInertia.x * _currentDirection.x;

            _currentTheta = _sphericalTransform.theta += _currentInertia.y * _currentDirection.y;
            _currentTheta = Mathf.Clamp(_currentTheta, _minTheta, _maxTheta);

            _sphericalTransform.theta = _currentTheta;
        }

        protected virtual void LateUpdate()
        {
            if (!_isActive)
                return;

            _sphericalTransform.transform.LookAt(_origin);
        }

        protected void ManageHorizontalMotion(bool isLeft, bool isRight)
        {
            if (isLeft)
            {
                if (_horizontalFrictionTween != null)
                    _horizontalFrictionTween.Kill();

                if (_currentDirection.x == 1)
                    _currentInertia.x = 0;

                _currentDirection.x = -1;
                _currentInertia.x += _acceleration;
                _currentInertia.x = Mathf.Clamp(_currentInertia.x, 0f, _maxInertia);
            }
            else if (isRight)
            {
                if (_horizontalFrictionTween != null)
                    _horizontalFrictionTween.Kill();

                if (_currentDirection.x == -1)
                    _currentInertia.x = 0;

                _currentDirection.x = 1;
                _currentInertia.x += _acceleration;
                _currentInertia.x = Mathf.Clamp(_currentInertia.x, 0f, _maxInertia);
            }
            else if (_currentInertia.x > 0 && !_horizontalFrictionTween.active)
            {
                _horizontalFrictionTween = DOVirtual.Float(
                    _currentInertia.x,
                    0f,
                    _friction,
                    (float value) =>
                    {
                        _currentInertia.x = value;
                    }
                );
            }
        }

        protected void ManageVerticalMotion(bool isUp, bool isDown)
        {
            if (isUp)
            {
                if (_vertialFrictionTween != null)
                    _vertialFrictionTween.Kill();

                if (_currentDirection.y == -1)
                    _currentInertia.y = 0;

                _currentDirection.y = 1;
                _currentInertia.y += _acceleration;
                _currentInertia.y = Mathf.Clamp(_currentInertia.y, 0f, _maxInertia);
            }
            else if (isDown)
            {
                if (_vertialFrictionTween != null)
                    _vertialFrictionTween.Kill();

                if (_currentDirection.y == 1)
                    _currentInertia.y = 0;

                _currentDirection.y = -1;
                _currentInertia.y += _acceleration;
                _currentInertia.y = Mathf.Clamp(_currentInertia.y, 0f, _maxInertia);
            }
            else if (_currentInertia.y > 0 && !_vertialFrictionTween.active)
            {
                _vertialFrictionTween = DOVirtual.Float(
                    _currentInertia.y,
                    0f,
                    _friction,
                    (float value) =>
                    {
                        _currentInertia.y = value;
                    }
                );
            }
        }

		#endregion
    }
}
