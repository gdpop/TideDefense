using UnityEngine;

namespace PierreMizzi.MouseInteractable
{
    public interface IClickable
    {
        void onLeftClick(RaycastHit hit);
        // public void OnClickDown;
        // public void OnClickUp;
        // public void OnDoubleClick;
    }
}
