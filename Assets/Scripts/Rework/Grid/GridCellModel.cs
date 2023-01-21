namespace TideDefense
{
    using PierreMizzi.Grid;

    public class GridCellModel : AGridCell
    {
        public Rempart rempart;
        public BeachTool currentTool = null;

        public bool isEmpty
        {
            get { return rempart == null && currentTool == null; }
        }

    }
}
