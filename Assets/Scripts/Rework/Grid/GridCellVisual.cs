namespace TideDefense
{
    using PierreMizzi.MouseInteractable;
    using UnityEngine;

    /*
        GridModel + GridCellVisual

    */

    public class GridCellVisual : MonoBehaviour
    {
        #region Fields

        private GridManager _gridManager = null;

        [SerializeField]
        protected Vector2Int _coords = new Vector2Int();
        public Vector2Int coords
        {
            get { return _coords; }
        }

        #region Hints

        public GameObject _diggableHintSprite = null;
        public GameObject _buildableHintSprite = null;

        #endregion

        #region Clickable

        // [SerializeField]
        // private Clickable _clickable = null;

        #endregion

        #region HoldClickable

        // [SerializeField] private Clickable _clickable = null;

        [SerializeField]
        private HoldClickable _holdClickable = null;

        #endregion

        #region Hoverable

        [SerializeField]
        private Hoverable _hoverable = null;

        #endregion

        #endregion

        #region Methods

        public void Initialize(GridManager gridManager, Vector2Int coords)
        {
            _gridManager = gridManager;
            _coords = coords;

            // _clickable.OnLeftClick += CallbackOnLeftClick;

            InitiliazeHoldClickable();
            // InitiliazeHoverable();
        }

        #region Hints

        public void DisplayDiggableHints()
        {
            _diggableHintSprite.SetActive(true);
        }

        public void HideDiggableHints()
        {
            _diggableHintSprite.SetActive(false);
        }

        public void DisplayBuildableHints()
        {
            _buildableHintSprite.SetActive(true);
        }

        public void HideBuildableHints()
        {
            _buildableHintSprite.SetActive(false);
        }

        #endregion

        #region Clickable

        public void CallbackOnLeftClick(RaycastHit hit)
        {
            _gridManager.CallbackLeftClick(this, hit);
        }

        #endregion

        #region HoldClickable

        public void InitiliazeHoldClickable()
        {
            if (_holdClickable != null)
            {
                // Left Click
                HoldClickBehaviour holdLeftClickBehaviour = _holdClickable.GetBehaviour(
                    InteractableManager.MOUSE_LEFT
                );

                holdLeftClickBehaviour.onClick += CallbackOnLeftClick;

                holdLeftClickBehaviour.onStartHoldClick += CallbackStartHoldClickLeft;
                holdLeftClickBehaviour.onProgressHoldClick += CallbackProgressHoldClickLeft;
                holdLeftClickBehaviour.onCompleteHoldClick += CallbackCompleteHoldClickLeft;
                holdLeftClickBehaviour.onCancelHoldClick += CallbackCancelHoldClickLeft;
            }
        }



        public void CallbackStartHoldClickLeft()
        {
            _gridManager.CallbackStartHoldClickLeft(this);
        }

        public void CallbackProgressHoldClickLeft(float progress)
        {
            _gridManager.CallbackProgressHoldClickLeft(this, progress);
        }

        public void CallbackCompleteHoldClickLeft()
        {
            _gridManager.CallbackCompleteHoldClickLeft(this);
        }

        public void CallbackCancelHoldClickLeft()
        {
            _gridManager.CallbackCancelHoldClickLeft(this);
        }

        #endregion

        #region Hoverable

        public void InitiliazeHoverable()
        {
            if (_holdClickable != null)
            {
                _hoverable.onHoverEnter += CallbackOnHoverEnter;
                _hoverable.onHover += CallbackOnHover;
                _hoverable.onHoverExit += CallbackOnHoverExit;
            }
        }

        private void CallbackOnHoverEnter(RaycastHit hit)
        {
            _gridManager.CallbackOnHoverEnter(this, hit);
        }

        private void CallbackOnHover(RaycastHit hit)
        {
            _gridManager.CallbackOnHover(this, hit);
        }

        private void CallbackOnHoverExit()
        {
            _gridManager.CallbackOnHoverExit(this);
        }

        #endregion

        #endregion
    }
}
