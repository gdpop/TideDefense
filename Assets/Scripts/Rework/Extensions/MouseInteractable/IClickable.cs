using UnityEngine;

namespace PierreMizzi.MouseInteractable
{
    public interface IClickable : IInteractable
    {
        bool isClickable { get; set; }

        void OnLeftClick(RaycastHit hit);
        
        void OnStartLongLeftClick();
        void OnProgerssLongLeftClick(float progress);
        void OnCancelLongLeftClick();

        // public void OnClickDown;
        // public void OnClickUp;
        // public void OnDoubleClick;
    }
}
