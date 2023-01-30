using System.Collections.Generic;
using UnityEngine;

namespace TideDefense
{
    public class RempartMould : MouldTool
    {
		#region Fields

		#region Mould

        [Header("Mould")]
        [SerializeField]
        protected List<Fortification> _mouldedShapes = new List<Fortification>();

        public new Fortification mouldedShape
        {
            get { return null; }
        }

		#endregion

		#region Manage Rotation

        // Scrolling Settings
        private float _scrollingValue = 0f;

        [SerializeField]
        private float _scrollingSpeed = 1f;

        // Rotation Settings
        [SerializeField]
        private float _stepTreshold = 2f;
        private int _currentStep = 0;
        private int _lastStep = 0;

		#endregion

		#endregion

		#region Methods

		#region MonoBehaviour

        protected virtual void Update()
        {
            ManageRotation();
        }

		#endregion

		#region Manage rotation

        public void ManageRotation()
        {
            _scrollingValue += Input.mouseScrollDelta.y * _scrollingSpeed;

            _currentStep = Mathf.FloorToInt(_scrollingValue / _stepTreshold);

            if (_currentStep != _lastStep)
            {
                Debug.Log($"New step : from {_lastStep} to{_currentStep}");
            }
        }

		#endregion

		#endregion
    }
}
