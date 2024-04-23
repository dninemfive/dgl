namespace d9.dgl;
public readonly struct Board<T>(T[,] values)
    where T : struct
{
    private readonly T[,] _values = (T[,])values.Clone();
    public int Width => _values.GetLength(0);
    public int Height => _values.GetLength(1);
    public T this[int x, int y]
        => _values[x, y];
    public bool InBounds(int x, int y)
        => x >= 0 && x < Width && y >= 0 && y < Height;
    public IEnumerable<T> NeighborsOf(int originalX, int originalY)
    {
        for(int xOffset = -1; xOffset <= 1; xOffset++)
            for(int yOffset = -1; yOffset <= 1; yOffset++)
            {
                if(xOffset == yOffset)
                    continue;
                (int x, int y) = (originalX + xOffset, originalY + yOffset);
                if (InBounds(x, y))
                    yield return this[x, y];
            }
    }
    public static implicit operator T[,](Board<T> board)
        // https://stackoverflow.com/a/39281887
        => (T[,])board._values.Clone();
    public static implicit operator Board<T>(T[,] values)
        => new(values);
}