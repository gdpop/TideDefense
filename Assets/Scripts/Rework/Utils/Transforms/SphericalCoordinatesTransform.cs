using UnityEngine;

namespace VirtuoseReality.Utils.TransformTools
{

	[ExecuteInEditMode]
	public class SphericalCoordinatesTransform : MonoBehaviour
	{

		#region Fields

		[SerializeField] private bool _isUpdating = true;
		public bool isUpdating { get { return _isUpdating; } set { _isUpdating = value; } }

		[SerializeField] private float _d = 1f;
		[SerializeField, Range(0f, 360f)] private float _phi = 0f;
		[SerializeField, Range(-90f, 90f)] private float _theta = 0f;

		public float d { get { return _d; } set { _d = value; } }
		public float phi { get { return _phi; } set { _phi = value; } }
		public float theta { get { return _theta; } set { _theta = value; } }

		[SerializeField] private Transform _origin = null;
		[SerializeField] private bool _CartesianToSpherical = false;

		#endregion

		#region Methods

		private void Update()
		{
			if (_CartesianToSpherical || !_isUpdating) return;

			if(_origin != null)
				transform.position = SphericalCoordinates(_origin.position, _d, _phi, _theta);
			else 	
				transform.position = SphericalCoordinates(Vector3.zero, _d, _phi, _theta);

		}

		public static Vector3 SphericalCoordinates(Vector3 origin, float d, float phi, float theta)
		{
			// Rotation around Y axis
			Vector3 phiVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * phi), 0f, Mathf.Sin(Mathf.Deg2Rad * phi));

			Vector3 crossPhiVector = Vector3.Cross(phiVector, Vector3.up);
            theta = Mathf.Clamp(theta, -90, 90);
			Quaternion thetaRotation = Quaternion.AngleAxis(theta, crossPhiVector);

			return origin + (thetaRotation * phiVector) * d;
		}

		public void Copy(SphericalCoordinatesTransform copyTransform)
		{
			d = copyTransform.d;
			phi = copyTransform.phi;
			theta = copyTransform.theta;
		}

		[ContextMenu("Cartesian to Spherical")]
		public void CartesianToSpherical()
        {
			if(transform.position == Vector3.zero)
            {
				_d = 0; _phi = 0; _theta = 0; return;
            }
			_d = _origin != null ? Vector3.Distance(_origin.position, transform.position) : Vector3.Distance(Vector3.zero, transform.position);
			_theta =Mathf.Asin(transform.position.y/_d)* Mathf.Rad2Deg;

			if (transform.position.x >= 0 && transform.position.z > 0) _phi = Mathf.Atan(transform.position.z / transform.position.x) * Mathf.Rad2Deg;
			else if (transform.position.x < 0 && transform.position.z >= 0) _phi = (Mathf.Atan(Mathf.Abs( transform.position.x / transform.position.z)) + Mathf.PI/2.0f) * Mathf.Rad2Deg;
			else if (transform.position.x <= 0 && transform.position.z < 0) _phi = (Mathf.Atan(Mathf.Abs(transform.position.z / transform.position.x)) + Mathf.PI )* Mathf.Rad2Deg;
			else _phi =( Mathf.Atan(Mathf.Abs(transform.position.x / transform.position.z)) + 3* Mathf.PI / 2.0f )* Mathf.Rad2Deg;
			


		}
		#endregion

	}
}