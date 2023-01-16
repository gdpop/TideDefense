using UnityEngine;

namespace PierreMizzi.MouseInteractable
{
    public interface IClickable : IInteractable
    {
        void OnLeftClick(RaycastHit hit);
        // public void OnClickDown;
        // public void OnClickUp;
        // public void OnDoubleClick;
    }
}
