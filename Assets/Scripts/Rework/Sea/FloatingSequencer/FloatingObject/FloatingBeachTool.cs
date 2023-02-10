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

        public override void Initialize(SeaManager seaManager)
        {
            base.Initialize(seaManager);
            _tool.StartFloating();
            Debug.Log("Test");

            _objectContainer.transform.rotation = UtilsClass.RandomRotation();
        }

        protected override void WashUpComplete()
        {
            _tool.transform.SetParent(_seaManager.washedUpContainer);
			_tool.WashUpComplete();
			
            _seaManager.DestroyFloatingObject(this);
        }

		#endregion
    }
}
