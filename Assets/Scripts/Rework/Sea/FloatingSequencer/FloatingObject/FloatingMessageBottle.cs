using UnityEngine;

namespace TideDefense
{
    public class FloatingMessageBottle : FloatingObject
    {
		#region Fields

        [SerializeField]
        private MessageBottle _bottle = null;

		#endregion

		#region Methods

        public void Initialize(SeaManager seaManager, MessageBottleData data)
        {
            base.Initialize(seaManager);
            _bottle.Initialize(data);
        }

        protected override void WashUpComplete()
        {
            _bottle.transform.SetParent(_seaManager.washedUpContainer);
            _bottle.SetWashedUp();
            _seaManager.DestroyFloatingObject(this);
        }

		#endregion
    }
}
