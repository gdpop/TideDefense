using UnityEngine;

namespace PierreMizzi.MouseInteractable
{
    public interface IClickable : IInteractable
    {
        bool isClickable { get; set; }

        void OnLeftClick(RaycastHit hit);

        // public void OnClickDown;
        // public void OnClickUp;
        // public void OnDoubleClick;
    }
}
