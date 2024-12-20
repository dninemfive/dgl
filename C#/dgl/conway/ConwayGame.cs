using d9.dgl.framework;

namespace d9.dgl.conway;
public class ConwayGame
{
    private readonly List<ConwayDiff> _diffs = [];
    public ConwayState CurrentState;
    public int Width => CurrentState.Width;
    public int Height => CurrentState.Height;
    public ConwayGame(int width, int height, double cellProbability)
    {
        Random random = new();
        ConwayCell[,] state = new ConwayCell[width, height];
        foreach((int x, int y) in state.AllPoints())
        {
            state[x, y] = random.NextDouble() < cellProbability;
        }
        CurrentState = state;
    }
    private async Task<ConwayState> Evolve()
    {        
        ConwayDiff diff = await Task.Run(CurrentState.Evolve);
        lock(_diffs)
            _diffs.Add(diff);
        CurrentState += diff;
        return CurrentState;
    }
    public async IAsyncEnumerable<ConwayState> Evolve(int times = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(times, 1, $"Cannot evolve {times} times!");
        for(int i = 0; i < times; i++)
            yield return await Evolve();
    }
}