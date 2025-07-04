namespace Bubbles.Spawn
{
    public class RectanglePattern : IBubbleRowPattern, IBubbleColorPattern
    {
        private int _rows;
        private int _columns;
        public BubbleColor ShapeColor { get; }

        public int RowCount => _rows;

        public RectanglePattern(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            
            ShapeColor = (BubbleColor)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(BubbleColor)).Length);
        }

        public int GetColumnsCount(int row) => _columns;

        public bool HasBubbleAt(int row, int column)
        {
            if (row < 0 || row >= _rows) return false;
            
            bool isEdgeRow = row == 0 || row == _rows - 1;
            bool isEdgeCol = column == 0 || column == _columns - 1;

            return isEdgeRow || isEdgeCol;
        }
    }
}