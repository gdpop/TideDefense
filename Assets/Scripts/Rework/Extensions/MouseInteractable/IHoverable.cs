namespace PierreMizzi.MouseInteractable
{
    using UnityEngine;

    public interface IHoverable : IInteractable
    {
        bool isHovered { get; }
        void OnHoverEnter(RaycastHit hit);
        void OnHoverExit();
        void OnHover(RaycastHit hit);
    }
}
