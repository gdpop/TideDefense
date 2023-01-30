namespace TideDefense
{
    using System.Collections.Generic;
    using PierreMizzi.TilesetUtils;
    using UnityEngine;
    using VirtuoseReality.Rendering;

    public class SandTower : Fortification
    {
        #region Fields

        #region Flag Pole

        [SerializeField]
        private FlagPole _flagPole = null;

        [SerializeField]
        protected List<MaterialPropertyBlockModifier> _materialPropertyBlocks =
            new List<MaterialPropertyBlockModifier>();

        #endregion

        #region Linking Remparts


        [SerializeField]
        private List<GameObject> _linkingRemparts = new List<GameObject>();

        #endregion

        #endregion

        #region Methods

        protected virtual void Awake()
        {
            _flagPole.Initialize(this);
        }

        public override void Initialize(
            FortificationManager manager,
            GridCellModel cellModel,
            float sandConcentration
        )
        {
            base.Initialize(manager, cellModel, sandConcentration);
            // ManageLinkingRemparts(-1);
        }

        #region Health

        public override void InflictDamage(float damageTaken)
        {
            _flagPole.RefreshFlagHeight(damageTaken);
            base.InflictDamage(damageTaken);
        }

        protected override void SetHealthFromSandConcentration(float sandConcentration)
        {
            base.SetHealthFromSandConcentration(sandConcentration);

            foreach (MaterialPropertyBlockModifier modifier in _materialPropertyBlocks)
                modifier.SetFloat(MouldTool.SAND_CONCENTRATION_PROPERTY, sandConcentration);

            _flagPole.RefreshFlagColor(_quality);
        }

        #endregion

        #region Linking Remparts

        public void RefreshLinkingRemparts(List<int> linkingRempartsIndex)
        {
            int count = _linkingRemparts.Count;
            for (int i = 0; i < count; i++)
            {
                _linkingRemparts[i].SetActive(linkingRempartsIndex.Contains(i));
            }
        }

        #endregion

        #endregion
    }
}
