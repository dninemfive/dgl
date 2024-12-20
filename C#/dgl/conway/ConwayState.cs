using System.Threading.Tasks;

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
    internal bool? Evolve(Point p)
    {
        int neighborCount = NeighborsOf(p).Count(x => x);
        foreach (ConwayRule rule in ConwayRules.All)
            if (rule(_grid[p], neighborCount) is ConwayCell cell && cell != _grid[p])
                return cell;
        return null;
    }
    private IEnumerable<Task<(Point p, bool? value)>> EvolutionTasks()
    {
        ConwayState _this = this;
        foreach (Point p in _grid.AllPoints)
            yield return Task.Run(() => (p, _this.Evolve(p)));
    }
    // https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.parallel.foreach?view=net-9.0
    // https://stackoverflow.com/a/56518630
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/iteration-statements#await-foreach
    public async IAsyncEnumerable<Point> Evolve()
    {
        List<Task<(Point p, bool? value)>> tasks = EvolutionTasks().ToList();
        while(tasks.Any())
        {
            Task<(Point p, bool? value)> task = await Task.WhenAny(tasks);
            (Point p, bool? value) = await task;
            tasks.Remove(task);
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