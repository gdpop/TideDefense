namespace TideDefense
{
    using DG.Tweening;
    using PierreMizzi.TilesetUtils;
    using UnityEngine;
    using VirtuoseReality.Rendering;

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

		#region Content

        [Header("Content Visual")]
        [SerializeField]
        private Transform _bucketContentVisual = null;

        [SerializeField]
        private Transform _contentVisualEmptyAnchor = null;

        [SerializeField]
        private Transform _contentVisualFullAnchor = null;

        public SandWaterFilling _content = new SandWaterFilling();

        [SerializeField]
        private float _maxQuantity = 1f;

        [SerializeField]
        private MaterialPropertyBlockModifier _contentVisualPropertyBlock = null;

        public static string SAND_CONCENTRATION_PROPERTY = "_SandConcentration";

        public bool isFull
        {
            get { return Mathf.Approximately(_content.quantity, _maxQuantity); }
        }

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
                    _contentVisualPropertyBlock.SetFloat(
                        SAND_CONCENTRATION_PROPERTY,
                        _content.sandConcentration
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

        public void Empty()
        {
            _content.quantity = 0;
            ResetContentVisual();
        }

        private void ResetContentVisual()
        {
            _bucketContentVisual.gameObject.SetActive(false);
        }

		#endregion

		#endregion
    }
}
