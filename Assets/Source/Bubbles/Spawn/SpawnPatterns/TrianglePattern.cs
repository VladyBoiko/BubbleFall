namespace Bubbles.Spawn
{
    public class TrianglePattern : IBubbleRowPattern, IBubbleColorPattern
    {
        private int _rows;
        private int _columns;
        public BubbleColor ShapeColor { get; }
        
        public int RowCount => _rows;

        public TrianglePattern(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            
            ShapeColor = (BubbleColor)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(BubbleColor)).Length);
        }

        public int GetColumnsCount(int row) => _columns;

        public bool HasBubbleAt(int row, int column)
        {
            if (row < 0 || row >= _rows) return false;
            
            int bubblesInRow = row * 2 + 1;
            int leftPadding = (_columns - bubblesInRow) / 2;

            return column >= leftPadding && column < leftPadding + bubblesInRow;
        }
    }
}