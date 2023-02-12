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

            _bottle.transform.rotation = Quaternion.Euler(20f, Random.Range(0, 359), 0);
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
