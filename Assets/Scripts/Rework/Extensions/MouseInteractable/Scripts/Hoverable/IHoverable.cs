namespace PierreMizzi.MouseInteractable
{
    using UnityEngine;

    public interface IHoverable : IInteractable
    {
        bool isHoverable { get; set; }
        bool isHovered { get; }
        void OnHoverEnter(RaycastHit hit);
        void OnHoverExit();
        void OnHover(RaycastHit hit);
    }
}
