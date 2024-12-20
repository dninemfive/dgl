namespace d9.dgl.conway;

public readonly struct ConwayState(ConwayGrid grid)
{
    private readonly ConwayGrid _grid = grid;
    public int Width => _grid.Width;
    public int Height => _grid.Height;
    public IEnumerable<ConwayCell> NeighborsOf(Point p)
    {
        foreach (Point neighbor in _grid.PointsAdjacentTo(p))
            yield return _grid[neighbor];
    }
    public ConwayDiff Evolve()
    {
        foreach (Point p in _grid.AllPoints)
        {
            (int x, int y) = p;
            int neighborCount = NeighborsOf(p).Count(x => x);
            foreach (ConwayRule rule in ConwayRules.All)
            {
                if (rule(_grid[p], neighborCount) is ConwayCell cell && cell != _grid[p])
                {
                    yield return p;
                    break;
                }
            }
        }
    }
    public static implicit operator ConwayState(ConwayCell[,] grid)
        => new(grid);
    public static ConwayState operator +(ConwayState state, ConwayDiff diff)
    {
        ConwayCell[,] result = state._grid;
        foreach ((int x, int y) in diff)
            result[x, y] = !state._grid[x, y];
        return result;
    }
    public IEnumerable<Point> LiveCells
    {
        get
        {
            foreach (Point point in _grid.AllPoints)
                if (_grid[point])
                    yield return point;
        }
    }
}