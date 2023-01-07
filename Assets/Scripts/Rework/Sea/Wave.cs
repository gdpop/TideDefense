using UnityEngine;
using UnityEngine.Collections;

namespace TideDefense
{
    public class Wave : MonoBehaviour
    {
        #region Fields

        private SeaManager _seaManager = null;

        public Action onDisappear = null;

        #endregion

        #region Methods

        public void Initialize(SeaManager seaManager)
        {
            _seaManager = seaManager;
            transform.position = _seaManager.currentTidePosition;
            StartCoroutine("DisappearBehaviour");
        }

        protected void LateUpdate()
        {
            if (_seaManager != null)
            {
                transform.position = _seaManager.currentTidePosition;
            }
        }

        private IEnumerator DisappearBehaviour()
        {
            yield return WaitForSeconds(2f);

            onDisappear.Invoke();

            yield return null;
        }

        #endregion
    }
}
