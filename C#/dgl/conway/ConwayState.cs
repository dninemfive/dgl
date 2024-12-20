using d9.aoc.core;
using d9.dgl.framework;
using ConwayCell = byte;
using ConwayGrid = d9.aoc.core.Grid<byte>;
using Point = d9.aoc.core.Point<int>;

namespace d9.dgl.conway;
internal class ConwayGame
{
    private List<State> _states = [];
    public State LatestState => _states.Last();
    public ConwayGame(int width, int height, double cellProbability)
    {
        Random random = new();
        ConwayCell[,] state = new ConwayCell[width, height];
        foreach((int x, int y) in state.AllPoints())
        {
            state[x, y] = (byte)(random.NextDouble() < cellProbability ? 1 : 0);
        }
        _states.Add(state);
    }
}
internal readonly struct State(ConwayGrid grid)
{
    internal readonly ConwayGrid _grid = grid;
    public IEnumerable<ConwayCell> NeighborsOf(Point p)
    {
        foreach (Point neighbor in _grid.PointsAdjacentTo(p))
            yield return _grid[neighbor];
    }
    public State Evolve()
    {
        ConwayCell[,] result = _grid;
        foreach(Point p in _grid.AllPoints)
        {
            (int x, int y) = p;
            int neighborCount = NeighborsOf(p).Select(x => (int)x)
                                              .Aggregate((x, y) => x + y);
            foreach(ConwayRule rule in ConwayRules.All)
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
    public static implicit operator State(ConwayCell[,] grid)
        => new(grid);
}
public delegate ConwayCell? ConwayRule(ConwayCell cell, int neighborCount);
public static class ConwayRules
{
    public static bool IsAlive(this ConwayCell cell)
        => cell > 0;
    public static readonly IEnumerable<ConwayRule> All = [
        DieByUnderpopulation,
        Live,
        DieByOverpopulation,
        Reproduce
    ];
    public static ConwayCell? DieByUnderpopulation(ConwayCell cell, int neighborCount)
        => cell.IsAlive() && (neighborCount is 0 or 1) ? 0 : null;
    public static ConwayCell? Live(ConwayCell cell, int neighborCount)
        => cell.IsAlive() && (neighborCount is 2 or 3) ? 1 : null;
    public static ConwayCell? DieByOverpopulation(ConwayCell cell, int neighborCount)
        => cell.IsAlive() && (neighborCount > 3) ? 0 : null;
    public static ConwayCell? Reproduce(ConwayCell cell, int neighborCount)
        => !cell.IsAlive() && neighborCount is 3 ? 1 : null;
}