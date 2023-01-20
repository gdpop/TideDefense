namespace TideDefense
{
    using DG.Tweening;
    using PierreMizzi.TilesetUtils;
    using UnityEngine;

    /*
    
        [Bucket dropped]

        [Bucket Grabbed]
        [ ] IF Ready To Build
            - Display construction grid

        Sand
        [ ] Hover water - Long Left Click
            - Fill with water
        [ ] Hover water - Long Right Click
            - Empty Bucket

        Water
        [x] Hover sand - Left Click
            - Drop

        [ ] Hover sand - Long Right Click - IF Ready to Build
            - Create Rempart
    
    */

    public class Bucket : BeachTool
    {
		#region Fields

		#region Diggable Hints

        [Header("Diggable Hints")]
        [SerializeField]
        private Transform _diggableHintsContainer = null;

		#endregion

		#region Content

        public SandWaterFilling _content = new SandWaterFilling();

        [SerializeField]
        private float _maxQuantity = 1f;

		#endregion

		#endregion

		#region Methods

        #region Beach Tools

        public override void Initialize(GameplayManager manager)
        {
            base.Initialize(manager);
            InitializeContent();
        }

        #endregion

		#region Mono Behaviour

        protected override void Start()
        {
            base.Start();
            HideDiggableHints();
        }

		#endregion

		#region Diggable Hints

        public void DisplayDiggableHints()
        {
            _diggableHintsContainer.gameObject.SetActive(true);
            ManageIndividualHints();
        }

        public void ManageIndividualHints()
        {
            Vector2Int checkCoords = new Vector2Int();
            GameObject diggableHint = null;
            bool isValidCoords = false;

            for (int i = 0; i < TilesetUtils.neighboorsCoordinatesEight.Count; i++)
            {
                checkCoords = currentGridCell.coords + TilesetUtils.neighboorsCoordinatesEight[i];
                isValidCoords = _manager.gridManager.gridModel.CheckValidCoordinates(checkCoords);

                diggableHint = _diggableHintsContainer.GetChild(i).gameObject;
                diggableHint.SetActive(isValidCoords);
            }
        }

        public void HideDiggableHints()
        {
            _diggableHintsContainer.gameObject.SetActive(false);
        }

		#endregion

		#region Content

        public void FillBucket(SandWaterFilling added)
        {
            // If what's added is too much, we only take what we need to fill the bucket
            // added.sandConcentration is the same
            SandWaterFilling fromContent = new SandWaterFilling(
                _content.quantity,
                _content.sandConcentration
            );

            if (added.quantity + _content.quantity > _maxQuantity)
                added.quantity = _maxQuantity - _content.quantity;

            _content = _content + added;
            RefreshContentVisual(fromContent);
            // Debug.Log("_content");
            // Debug.Log(_content.ToString());
        }

        [Header("Content Visual")]
        [SerializeField]
        private Transform _bucketContentVisual = null;

        [SerializeField]
        private Transform _contentVisualEmptyAnchor = null;

        [SerializeField]
        private Transform _contentVisualFullAnchor = null;

        private void InitializeContent()
        {
            _content = new SandWaterFilling();
            _bucketContentVisual.gameObject.SetActive(false);
            _bucketContentVisual.localPosition = GetContentVisualLocalPosFromQuantity(
                _content.quantity
            );
        }

        public void RefreshContentVisual(SandWaterFilling fromContent)
        {
            DOVirtual.Float(
                fromContent.quantity,
                _content.quantity,
                1f,
                (float value) =>
                {
                    _bucketContentVisual.localPosition = GetContentVisualLocalPosFromQuantity(
                        value
                    );
                    _bucketContentVisual.gameObject.SetActive(value > 0.02f);
                }
            );
        }

        private Vector3 GetContentVisualLocalPosFromQuantity(float quantity)
        {
            return Vector3.Lerp(
                _contentVisualEmptyAnchor.localPosition,
                _contentVisualFullAnchor.localPosition,
                quantity
            );
        }

		#endregion

		#endregion
    }
}
