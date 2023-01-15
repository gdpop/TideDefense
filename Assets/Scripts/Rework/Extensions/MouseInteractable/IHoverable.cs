namespace PierreMizzi.MouseInteractable
{
    using UnityEngine;

    public interface IHoverable
    {
        bool isHovered { get; }
        void OnHoverEnter(RaycastHit hit);
        void OnHoverExit();
        void OnHover(RaycastHit hit);
    }
}
