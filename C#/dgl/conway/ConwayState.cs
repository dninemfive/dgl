namespace d9.dgl.conway;

public readonly struct ConwayState(ConwayGrid grid)
{
    public readonly ConwayGrid _grid = grid;
    public IEnumerable<ConwayCell> NeighborsOf(Point p)
    {
        foreach (Point neighbor in _grid.PointsAdjacentTo(p))
            yield return _grid[neighbor];
    }
    public ConwayState Evolve()
    {
        ConwayCell[,] result = _grid;
        foreach (Point p in _grid.AllPoints)
        {
            (int x, int y) = p;
            int neighborCount = NeighborsOf(p).Select(x => (int)x)
                                              .Aggregate((x, y) => x + y);
            foreach (ConwayRule rule in ConwayRules.All)
            {
                if (rule(_grid[p], neighborCount) is ConwayCell cell)
                {
                    result[p.X, p.Y] = cell;
                    break;
                }
            }
        }
        return result;
    }
    public static implicit operator ConwayState(ConwayCell[,] grid)
        => new(grid);
}