namespace Bubbles.Spawn
{
    public class HexPattern : IBubbleRowPattern
    {
        private int _rows;
        private int _columns;

        public int RowCount => _rows;

        public HexPattern(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
        }

        public int GetColumnsCount(int row)
        {
            return row % 2 == 0 ? _columns : _columns - 1;
        }

        public bool HasBubbleAt(int row, int column)
        {
            int maxCols = GetColumnsCount(row);
            return column >= 0 && column < maxCols;
        }
    }
}