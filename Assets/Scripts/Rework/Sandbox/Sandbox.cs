using System.Collections;
using System.Collections.Generic;
using CodesmithWorkshop.Useful;
using DG.Tweening;
using TideDefense;
using ToolBox.Pools;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
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

    // #region Manage Rotation

    // private bool _updateRotation = false;

    // public bool updateRotation
    // {
    //     get { return _updateRotation; }
    //     set { _updateRotation = value; }
    // }

    // // Scrolling Settings
    // private float _scrollingValue = 0f;

    // [SerializeField]
    // private float _scrollingSpeed = 0.1f;

    // // Rotation Settings
    // private int _currentStep = 0;
    // private int _lastStep = 0;

    // [SerializeField]
    // private float _rotationSpeed = 0.5f;

    // [SerializeField]
    // private List<float> _allowedRotation = new List<float>();

    // private int _allowedRotationIndex = 0;

    // #endregion


    // #region MonoBehaviour

    // protected virtual void Update()
    // {
    //     if (_updateRotation)
    //         ManageRotation();
    // }

    // #endregion

    // #region Manage rotation

    // public void ManageRotation()
    // {
    //     _scrollingValue += Input.mouseScrollDelta.y * _scrollingSpeed;

    //     _currentStep = Mathf.FloorToInt(_scrollingValue / 2f);

    //     if (_currentStep != _lastStep)
    //     {
    //         _lastStep = _currentStep;
    //         Rotate();
    //     }
    // }

    // public void Rotate()
    // {
    //     _allowedRotationIndex = Mathf.FloorToInt(Mathf.Abs(_currentStep) % _allowedRotation.Count);
    //     Vector3 eulerAngles = new Vector3(0f, _allowedRotation[_allowedRotationIndex], 0f);
    //     transform.DOLocalRotate(eulerAngles, _rotationSpeed).SetEase(Ease.InQuad);
    // }

    // #endregion

    #endregion

    #region SeaManager+Ressource

    // private Bounds _floatingSpawnZone = new Bounds();

    // [SerializeField]
    // private FloatingObject _floatingPrefab = null;

    // [SerializeField]
    // private FloatingObjectSettings _floatingSettings = null;
    // public FloatingObjectSettings floatingSettings
    // {
    //     get { return _floatingSettings; }
    // }

    // [SerializeField]
    // private Transform _originSpawnZone = null;

    // private Vector3 _submergedOffset;

    // private void OnEnable()
    // {
    //     _submergedOffset = new Vector3(0, _floatingSettings.submergedOffsetY, 0);
    // }

    // private void Update()
    // {
    //     UpdateFloatingSpawnZone();
    //     if(Input.GetKeyDown(KeyCode.K))
    //     {
    //         SpawnFloatingObject();
    //     }
    // }

    // private void SpawnFloatingObject()
    // {
    //     FloatingObject floating = Instantiate(_floatingPrefab, GetSpawnPosition(), Quaternion.identity, _originSpawnZone);
    //     floating.Initialize(null);
    // }

    // private void UpdateFloatingSpawnZone()
    // {
    //     _floatingSpawnZone.center = _originSpawnZone.position;
    //     _floatingSpawnZone.extents = new Vector3(
    //         _floatingSettings.floatingSpawnZoneDimensions.x,
    //         0f,
    //         _floatingSettings.floatingSpawnZoneDimensions.y
    //     );
    // }

    // public Vector3 GetSpawnPosition()
    // {
    //     return _floatingSpawnZone.RandomPosition() + _submergedOffset;
    // }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(_originSpawnZone.position, 0.2f);
    //     Gizmos.DrawWireCube(_floatingSpawnZone.center, _floatingSpawnZone.extents * 2f);
    // }

    #endregion

    #region UI BUILDER TEST

    [SerializeField]
    private UIDocument _document = null;

    private VisualElement _background = null;

    private const string VA_BACKGROUND = "BACKGROUND";
    private const string VA_BOX_CONTAINER = "BoxContainer";
    private const string ANIM_BACKGROUND_DISPLAY = "animation-background-display";

    [ContextMenu("TestAddClassForTransition")]
    public void TestAddClassForTransition() {

        VisualElement root = _document.rootVisualElement;

        _background = root.Query<VisualElement>(VA_BOX_CONTAINER);

        _background.ToggleInClassList(ANIM_BACKGROUND_DISPLAY);
     }


     private const string VA_BUTTON = "Button";
     private const string ANIM_BUTTON_ANIMATE = "animate";

    [ContextMenu("TestAddClassForTransitionButton")]
    public void TestAddClassForTransitionButton() {

        VisualElement root = _document.rootVisualElement;

        VisualElement button = root.Query<VisualElement>(VA_BUTTON);

        button.schedule.Execute(() => button.AddToClassList(ANIM_BUTTON_ANIMATE)).StartingIn(100);
     }

         [ContextMenu("TestAddClassForTransitionButton - Out")]
    public void TestAddClassForTransitionButtonOut() {

        VisualElement root = _document.rootVisualElement;

        VisualElement button = root.Query<VisualElement>(VA_BUTTON);

        button.schedule.Execute(() => button.ToggleInClassList(ANIM_BUTTON_ANIMATE)).StartingIn(100);
     }

    #endregion
}
