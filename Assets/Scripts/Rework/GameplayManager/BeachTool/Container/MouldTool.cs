namespace TideDefense
{
    using System;
    using System.Collections.Generic;
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

    public class MouldTool : BeachTool
    {
		#region Fields

		#region Content

        [Header("Content Visual")]
        [SerializeField]
        protected Transform _bucketContentVisual = null;

        [SerializeField]
        protected Transform _contentVisualEmptyAnchor = null;

        [SerializeField]
        protected Transform _contentVisualFullAnchor = null;

        protected SandWaterFilling _content = new SandWaterFilling();

        public SandWaterFilling content
        {
            get { return _content; }
        }

        [SerializeField]
        protected float _maxQuantity = 1f;

        [SerializeField]
        protected MaterialPropertyBlockModifier _contentVisualPropertyBlock = null;

        public static string SAND_CONCENTRATION_PROPERTY = "_SandConcentration";

        public bool isFull
        {
            get { return Mathf.Approximately(_content.quantity, _maxQuantity); }
        }

		#endregion

        #region Mould


        public Fortification mouldedShape
        {
            get { return null; }
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

        public virtual void Fill(SandWaterFilling added)
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
        }

        public virtual void Empty()
        {
            _content.quantity = 0;
            ResetContentVisual();
        }

        protected virtual void InitializeContent()
        {
            _content = new SandWaterFilling();

            _bucketContentVisual.gameObject.SetActive(false);
            _bucketContentVisual.localPosition = GetContentVisualLocalPosFromQuantity(
                _content.quantity
            );
        }

        public virtual void RefreshContentVisual(SandWaterFilling fromContent)
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

        protected Vector3 GetContentVisualLocalPosFromQuantity(float quantity)
        {
            return Vector3.Lerp(
                _contentVisualEmptyAnchor.localPosition,
                _contentVisualFullAnchor.localPosition,
                quantity
            );
        }

        protected virtual void ResetContentVisual()
        {
            _bucketContentVisual.gameObject.SetActive(false);
        }

		#endregion

        #region Mould

        public virtual Fortification GetCastedObject()
        {
            throw new NotImplementedException();
        }

		#endregion

		#endregion
    }
}
