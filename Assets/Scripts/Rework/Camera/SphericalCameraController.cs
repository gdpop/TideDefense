namespace TideDefense
{
    using System;
    using UnityEngine;
    using VirtuoseReality.Utils.TransformTools;
    using DG.Tweening;

    [ExecuteInEditMode]
    public class SphericalCameraController : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private Transform _origin = null;

        [Header("Controls values")]
        [SerializeField]
        private Vector2Int _currentDirection = new Vector2Int();

        [SerializeField]
        private Vector2 _currentInertia = new Vector2();

        [SerializeField]
        private float _maxInertia = 1f;

        [SerializeField]
        private float _acceleration = 0.01f;

        [SerializeField]
        private float _friction = 0.01f;

        private Tween _horizontalFrictionTween = null;
        private Tween _vertialFrictionTween = null;

		#region Theta

        [Header("Theta")]
        [SerializeField]
        private float _startTheta = 33f;

        [SerializeField]
        private float _minTheta = 20f;

        [SerializeField]
        private float _maxTheta = 75f;

        private float _currentTheta = 0f;

		#endregion

        [SerializeField]
        private SphericalCoordinatesTransform _sphericalTransform = null;

		#endregion

		#region Methods

        private void OnEnable()
        {
            _horizontalFrictionTween = DOVirtual.DelayedCall(0f, () => { });
            _horizontalFrictionTween.Complete();
            _vertialFrictionTween = DOVirtual.DelayedCall(0f, () => { });
            _vertialFrictionTween.Complete();
            
            _sphericalTransform.theta = _startTheta;
        }

        private void Update()
        {
            // Computes Inertia on x axis (Phi angle)
            ManageHorizontalMotion(Input.GetKey(KeyCode.Q), Input.GetKey(KeyCode.D));

            // Computes Inertia on y axis (Theta angle)
            ManageVerticalMotion(Input.GetKey(KeyCode.Z), Input.GetKey(KeyCode.S));

            _sphericalTransform.phi += _currentInertia.x * _currentDirection.x;

            _currentTheta = _sphericalTransform.theta += _currentInertia.y * _currentDirection.y;
            _currentTheta = Mathf.Clamp(_currentTheta, _minTheta, _maxTheta);

            _sphericalTransform.theta = _currentTheta;
        }

        private void LateUpdate()
        {
            _sphericalTransform.transform.LookAt(_origin);
        }

        private void ManageHorizontalMotion(bool isLeft, bool isRight)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                if (_horizontalFrictionTween != null)
                    _horizontalFrictionTween.Kill();

                if (_currentDirection.x == 1)
                    _currentInertia.x = 0;

                _currentDirection.x = -1;
                _currentInertia.x += _acceleration;
                _currentInertia.x = Mathf.Clamp(_currentInertia.x, 0f, _maxInertia);
            }
            else if (Input.GetKey(KeyCode.D))
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

        private void ManageVerticalMotion(bool isUp, bool isDown)
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

        #region Screen Border Control
            
        #endregion

		#endregion
    }
}
