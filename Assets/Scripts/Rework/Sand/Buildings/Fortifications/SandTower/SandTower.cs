namespace TideDefense
{
    using System.Collections.Generic;
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

        #region Health

        public override void InflictDamage(float damageTaken)
        {
            base.InflictDamage(damageTaken);
            _flagPole.RefreshFlagHeight();
        }

        protected override void SetHealthFromSandConcentration(float sandConcentration)
        {
            base.SetHealthFromSandConcentration(sandConcentration);

            foreach (MaterialPropertyBlockModifier modifier in _materialPropertyBlocks)
                modifier.SetProperty(MouldTool.SAND_CONCENTRATION_PROPERTY, sandConcentration);

            // Debug.Log($"{sandConcentration},{_quality}");
            _flagPole.RefreshFlagColor(_fortificationChannel.GetColorFromQuality(_quality));
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
