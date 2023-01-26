using UnityEngine;

namespace VirtuoseReality.Utils.TransformTools
{

	/// <summary>
	/// Component used to align a transform along the forward axis of target, see AlignWorldPosition for further details
	/// </summary>
	[ExecuteInEditMode]
	public class TargetAligner : MonoBehaviour
	{

		#region Fields

		[Header("Components")]
		public Transform target = null;
		public bool activeUpdate = true;

		[Header("Settings")]
		public float distance = 2f;
		public Vector2 offset = Vector2.zero;
		public bool copyYPosition = true;


		#endregion

		#region Methods

		#region MonoBehaviour

		private void OnEnable()
		{
			if (target == null)
				target = Camera.main?.transform;
		}

		private void Start()
		{
			if (target == null)
				target = Camera.main?.transform;
		}

		public void LateUpdate()
		{
			if (activeUpdate)
				transform.position = AlignedWorldPosition(target, distance, offset, copyYPosition);
		}

		/// <summary>
		/// Align a transform along the forward axis of target
		/// </summary>
		/// <param name="target">Transform to align to</param>
		/// <param name="distance">Distance between target and aligned position</param>
		/// <param name="offset">Offset on x and y axis</param>
		/// <param name="copyYPosition">Should the aligned position ignore the Y value and simply copy target.position.y</param>
		/// <returns></returns>
		public static Vector3 AlignedWorldPosition(Transform target, float distance, Vector2 offset, bool copyYPosition = false)
		{
			Vector3 alignedPosition = new Vector3();
			if (target != null)
			{
				alignedPosition = target.position + target.forward * distance;

				if (copyYPosition)
					alignedPosition.y = target.position.y;

				alignedPosition += target.right * offset.x;
				alignedPosition += target.up * offset.y;

				return alignedPosition;
			}
			else
				return Vector3.zero;
		}


		#endregion

		#endregion

	}
}