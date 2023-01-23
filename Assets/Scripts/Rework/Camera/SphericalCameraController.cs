namespace TideDefense
{
    using UnityEngine;
    using VirtuoseReality.Utils.TransformTools;

    [ExecuteInEditMode]
    public class SphericalCameraController : MonoBehaviour
    {
		#region Fields

        [SerializeField]
        private Transform _origin = null;

        [SerializeField]
        private float _lerpingSpeed = 0f;

		#region Theta

        [Header("Theta")]
        [SerializeField]
        private float _startTheta = 33f;

        [SerializeField]
        private float _minTheta = 20f;

        [SerializeField]
        private float _maxTheta = 75f;

        [SerializeField]
        private float _thetaInertia = 10f;
        private float _currentThetaInertia = 0f;

        [SerializeField]
        private float _friction = 0.01f;

        private float _currentTheta = 33f;
        private float _thetaToReach = 0f;

		#endregion

        [SerializeField]
        private SphericalCoordinatesTransform _sphericalTransform = null;

		#endregion

		#region Methods

        private void OnEnable()
        {
            _sphericalTransform.theta = _startTheta;
            _currentTheta = _startTheta;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                _sphericalTransform.phi -= Time.deltaTime * _lerpingSpeed;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _sphericalTransform.phi += Time.deltaTime * _lerpingSpeed;
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                _currentThetaInertia = _thetaInertia;
                _currentTheta += Time.deltaTime * _lerpingSpeed;
                _currentTheta = Mathf.Clamp(_currentTheta, _minTheta, _maxTheta);
                _sphericalTransform.theta = _currentTheta;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _currentTheta -= Time.deltaTime * _lerpingSpeed;
                _currentTheta = Mathf.Clamp(_currentTheta, _minTheta, _maxTheta);
                _sphericalTransform.theta = _currentTheta;
            }
            if (_currentThetaInertia > 0)
            {
                _currentThetaInertia -= _friction;
                _currentTheta += Time.deltaTime * _currentThetaInertia;
                _currentTheta = Mathf.Clamp(_currentTheta, _minTheta, _maxTheta);
                _sphericalTransform.theta = _currentTheta;
            }

            _sphericalTransform.transform.LookAt(_origin);
        }

		#endregion
    }
}
