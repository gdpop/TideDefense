using System;
using UnityEngine;

namespace PierreMizzi.MouseInteractable
{
    public delegate void FloatEvent(float value);

    [Serializable]
    public class HoldClickBehaviour
    {
        [SerializeField]
        private string _name = null;

        [SerializeField]
        private int _mouseButtonID = 0;
        public int mouseButtonID
        {
            get { return _mouseButtonID; }
            set { _mouseButtonID = value; }
        }

        public HoldClickBehaviour()
        {
            OnStartHoldClick = ()=>{};
            OnProgressHoldClick = (float value) => { };
            OnCompleteHoldClick = ()=>{};
            OnCancelHoldClick = ()=>{};
        }

        public Action OnStartHoldClick;
        public FloatEvent OnProgressHoldClick;
        public Action OnCompleteHoldClick;
        public Action OnCancelHoldClick;
    }
}
