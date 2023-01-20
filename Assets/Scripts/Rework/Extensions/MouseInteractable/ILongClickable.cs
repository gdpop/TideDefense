namespace PierreMizzi.MouseInteractable
{
    public interface ILongClickable : IClickable
    {
        void OnStartLongLeftClick();
        void OnProgressLongLeftClick(float progress);
        void OnCompleteLongLeftClick();
        void OnCancelLongLeftClick();

		
    }
}
