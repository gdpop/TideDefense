namespace TideDefense
{
    using PierreMizzi.Grid;

    public class GridCellModel : AGridCell
    {
        public Building building;
        public BeachTool currentTool = null;

        public bool isEmpty
        {
            get { return building == null && currentTool == null; }
        }

    }
}
