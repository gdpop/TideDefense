using UnityEngine;
using System;

namespace TideDefense
{
	public class SeaManager : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private TimeChannel _timeChannel = null;

		[SerializeField]
		private Transform _seaTransform = null;

		[SerializeField]
		private float _seaSpreadOffset = 0.2f;

		#region Beach

		[SerializeField]
		private float _beachSlope = 5f;

		[SerializeField]
		private Transform _beachBottom = null;

		#endregion

		#region Tide

		/*
			Level of the tide is a distance between the bottom of the ocean and a point on a beach.
			It goes along the beach, not the y axis
		*/

		[SerializeField]
		private float _minTideLevel = 0.5f;

		[SerializeField]
		private float _maxTideLevel = 1.5f;

		/// <summary> 
		/// 
		/// </summary>
		private float _currentTideLevel = 0f;

		/// <summary> 
		/// "Time" value for the tide
		///</summary>
		private float _tideProgress = 0f;

		/// <summary> 
		/// Current phase of the tide : Ascending = true, descending = false
		///</summary>
		private bool _tidePhase = true;

		/// <summary> 
		/// Normalized value to determine the current progress of the phase
		/// For exemple when ascending, 0 = _minTideLevel, 1 = _maxTideLevel
		///</summary>
		private float _tidePhaseProgress = 0f;

		/// <summary>
		/// Exprimé en m/s
		///</summary>
		[SerializeField]
		private float _tideProgressSpeed = 0.2f;

		/// <summary> 
		///	Position of the edge of the tide	
		/// </summary>
		private Vector3 _currentTidePosition = null;
		public Vector3 currentTidePosition
		{
			get{
				return _currentTidePosition;
			}
		}

		#endregion

		#region Wave
			
		#endregion

		[Header("Wave")]
		[SerializeField] private GameObject _wavePrefab = null;

		private Wave _currentWave = null;

		#endregion

		#region Methods


		#region MonoBehaviour

		protected void Awake()
		{
			// _beachSlope = Mathf.Abs(_beachBottom.rotation.eulerAngles.x);
			Debug.Log("BeachSlope : " + _beachSlope);
		}

		protected void Start()
		{
			if (_timeChannel != null)
			{
				_timeChannel.onUpdateCurrentDeltaTime += CallbackUpdateCurrentDeltaTime;
			}
		}

		#endregion

		#region Tide
			
		protected void CallbackUpdateCurrentDeltaTime(float currentDeltaTime)
		{
			_tideProgress += currentDeltaTime * _tideProgressSpeed;

			_tidePhase = Convert.ToBoolean(1 - (int)Mathf.Floor(_tideProgress % 2));
			_tidePhaseProgress = (1.0f / Mathf.PI) * Mathf.Acos(Mathf.Sin(Mathf.PI * (_tideProgress + 0.5f)));

			// Debug.Log($"_tidePhase : {_tidePhase} | _tidePhaseProgress : {_tidePhaseProgress}");

			_currentTideLevel = Mathf.Lerp(_minTideLevel, _maxTideLevel, _tidePhaseProgress);

			_seaTransform.position =
				_beachBottom.position
				+ new Vector3(0f, TideProgressToSeaHeight(_currentTideLevel), 0f);

			_seaTransform.localScale = new Vector3(
				10f,
				1f,
				TideProgressToSeaSpread(_currentTideLevel)
			);

			_currentTidePosition = _beachBottom.position + _beachBottom.forward * _currentTideLevel;
		}

		/// <summary>
		/// Give the height of the sea along the y axis when given the tide level
		///</summary>
		private float TideProgressToSeaHeight(float tideLevel)
		{
			return Mathf.Cos(Mathf.Deg2Rad * (90.0f - _beachSlope)) * tideLevel;
		}

		/// <summary>
		///	Give the spread(scale) of the sea when given the tide level
		///</summary>
		private float TideProgressToSeaSpread(float tideLevel)
		{
			return _seaSpreadOffset + Mathf.Sin(Mathf.Deg2Rad * (90.0f - _beachSlope)) * tideLevel;
		}

		#endregion

		#region Wave
		
		[ContextMenu("Create Wave")]
		private void CreateWave()
		{
			_currentWave = Object.Instantiate(_wavePrefab) as Wave;
			_currentWave.Initialize(this);
			_currentWave.onDisappear += CallbackDestroyCurrentWave;
		}

		public void CallbackDestroyCurrentWave()
		{
			_currentWave -= CallbackDestroyCurrentWave;
			Destroy(_currentWave);
			_currentWave = null;
		}
			
		#endregion

		#endregion
	}
}
