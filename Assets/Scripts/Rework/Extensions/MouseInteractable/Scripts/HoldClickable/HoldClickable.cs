namespace PierreMizzi.MouseInteractable
{
    using System.Collections.Generic;
    using UnityEngine;

    public class HoldClickable : MonoBehaviour
    {
        [SerializeField]
        private bool _isInteractable = true;
        public bool isInteractable
        {
            get { return _isInteractable; }
            set { _isInteractable = value; }
        }

        [SerializeField]
        private List<HoldClickBehaviour> _behaviours = null;

        public HoldClickBehaviour GetBehaviour(int mouseButtonID)
        {
            return _behaviours.Find(behaviour => behaviour.mouseButtonID == mouseButtonID);
        }
    }
}
