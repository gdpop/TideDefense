using UnityEngine;

namespace TideDefense
{

	using CodesmithWorkshop.Useful;

    public class FloatingBeachTool : FloatingObject
    {
		#region Fields

        [SerializeField]
        private BeachTool _tool = null;

		#endregion

		#region Methods

        public void Initialize(SeaManager seaManager, MessageBottleData data)
        {
            base.Initialize(seaManager);

            _objectContainer.transform.rotation = UtilsClass.RandomRotation();
        }

        protected override void WashUpComplete()
        {
            _tool.transform.SetParent(_seaManager.washedUpContainer);
			_tool.InitializeWashedUp();
			
            _seaManager.DestroyFloatingObject(this);
        }

		#endregion
    }
}
