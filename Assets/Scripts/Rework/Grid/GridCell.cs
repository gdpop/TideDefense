namespace TideDefense
{
    using PierreMizzi.Grid;

    public class GridCell : AGridCell
    {
        public Rempart rempart;
        public BeachTool currentTool = null;

        public bool isEmpty
        {
            get { return rempart == null && currentTool == null; }
        }

    }
}
