using System.Collections;
using System.Collections.Generic;
using PierreMizzi.MouseInteractable;
using UnityEngine;

namespace TideDefense
{
    public class MessageBottle : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private UIChannel _UIChannel = null;

        [SerializeField]
        private HoldClickable _clickable = null;

        private MessageBottleData _data = null;

        #endregion

        #region Methods

        #region MonoBehaviour

        private void Start()
        {
            InitializeClickable();
        }

        #endregion

        public void Initialize(MessageBottleData data)
        {
            _data = data;
            _clickable.isInteractable = false;
        }

        public void InitializeClickable()
        {
            HoldClickBehaviour behaviour = _clickable.GetBehaviour(InteractableManager.MOUSE_LEFT);

            behaviour.onClick += CallbackOnClick;
        }

        public void SetWashedUp()
        {
            _clickable.isInteractable = true;
        }

        private void CallbackOnClick(RaycastHit hit)
        {
            Debug.Log("Clicked MESSAGE BOTTLE !");
            _UIChannel.onDisplayMessageBottle.Invoke(_data);
        }

        #endregion
    }
}
