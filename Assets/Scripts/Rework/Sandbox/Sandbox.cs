using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ToolBox.Pools;
using UnityEngine;

public class Sandbox : MonoBehaviour
{
    #region Test Pooling Object

    // [SerializeField]
    // private GameObject _foundationPrefab = null;

    // private GameObject _usedFoundationPrefab = null;

    // private void InitializeRempartFoundation()
    // {
    //     _foundationPrefab.gameObject.Populate(12);
    // }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         if (_usedFoundationPrefab == null)
    //             _usedFoundationPrefab = _foundationPrefab.Reuse(
    //                 transform.position,
    //                 transform.rotation
    //             );
    //     }
    //     else if (Input.GetKeyDown(KeyCode.D))
    //     {
    //         if (_usedFoundationPrefab != null)
    //         {
    //             _usedFoundationPrefab.Release();
    //             _usedFoundationPrefab = null;
    //         }
    //     }
    // }


    #endregion

    #region Test Step Rotation

    #region Manage Rotation

    // Scrolling Settings
    private float _scrollingValue = 0f;

    [SerializeField]
    private float _scrollingSpeed = 0.01f;

    // Rotation Settings
    private int _currentStep = 0;
    private int _lastStep = 0;

    private int _direction = 0;

    [SerializeField] private float _rotationSpeed = 0.5f;

    [SerializeField]
    private List<float> _allowedRotation = new List<float>();

    private int _allowedRotationIndex = 0;

    #endregion


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

        _currentStep = Mathf.FloorToInt(_scrollingValue / 2f);

        if (_currentStep != _lastStep)
        {
            _direction = _currentStep - _lastStep;
            Debug.Log($"New step : from {_lastStep} to{_currentStep} | direction {_direction}");
            _lastStep = _currentStep;
            Rotate();
        }
    }

    public void Rotate()
    {
        _allowedRotationIndex = Mathf.FloorToInt(Mathf.Abs(_currentStep) % _allowedRotation.Count);
        Debug.Log($"allowedRotationIndex : { _allowedRotationIndex }");
        Vector3 eulerAngles = new Vector3(0f, _allowedRotation[_allowedRotationIndex], 0f);
        transform.DOLocalRotate(eulerAngles, _rotationSpeed).SetEase(Ease.InQuad);
    }

		#endregion

    #endregion
}
