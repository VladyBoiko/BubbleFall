namespace Bubbles.Spawn
{
    public interface IBubbleRowPattern
    {
        int RowCount { get; }
        int GetColumnsCount(int row);
        bool HasBubbleAt(int row, int column);
    }
}