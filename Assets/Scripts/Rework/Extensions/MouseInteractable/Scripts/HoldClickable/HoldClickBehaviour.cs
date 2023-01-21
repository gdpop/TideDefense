using System;
using UnityEngine;

namespace PierreMizzi.MouseInteractable
{
    public delegate void FloatEvent(float value);

    public delegate void RaycastDelegate(RaycastHit hit);

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
            onClick = (RaycastHit hit) => { };

            onStartHoldClick = () => { };
            onProgressHoldClick = (float value) => { };
            onCompleteHoldClick = () => { };
            onCancelHoldClick = () => { };
        }

        public RaycastDelegate onClick;

        public Action onStartHoldClick;
        public FloatEvent onProgressHoldClick;
        public Action onCompleteHoldClick;
        public Action onCancelHoldClick;
    }
}
