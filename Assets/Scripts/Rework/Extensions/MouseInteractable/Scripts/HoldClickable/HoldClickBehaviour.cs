using System;
using UnityEngine;

namespace PierreMizzi.MouseInteractable
{
    public delegate void FloatEvent(float value);

    public delegate void RaycastDelegate(RaycastHit hit);

    // TODO Rename that into click Behaviour
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
            onMouseDown = (RaycastHit hit) => { };

            onStartHoldClick = () => { };
            onProgressHoldClick = (float value) => { };
            onCompleteHoldClick = () => { };
            onCancelHoldClick = () => { };
        }

        public RaycastDelegate onClick;
        public RaycastDelegate onMouseDown;

        public Action onStartHoldClick;
        public FloatEvent onProgressHoldClick;
        public Action onCompleteHoldClick;
        public Action onCancelHoldClick;
    }
}
