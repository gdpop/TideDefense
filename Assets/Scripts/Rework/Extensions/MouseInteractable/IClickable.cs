using UnityEngine;

namespace PierreMizzi.MouseInteractable
{
    public interface IClickable
    {
        void OnLeftClick(RaycastHit hit);
        // public void OnClickDown;
        // public void OnClickUp;
        // public void OnDoubleClick;
    }
}
