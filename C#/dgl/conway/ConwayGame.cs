using d9.dgl.framework;

namespace d9.dgl.conway;
internal class ConwayGame
{
    private readonly List<ConwayState> _states = [];
    public ConwayState LatestState => _states.Last();
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
    private async Task<ConwayState> Evolve()
    {        
        ConwayState next = await Task.Run(LatestState.Evolve);
        lock(_states)
            _states.Add(next);
        return next;
    }
    public async IAsyncEnumerable<ConwayState> Evolve(int times = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(times, 1, $"Cannot evolve {times} times!");
        for(int i = 0; i < times; i++)
            yield return await Evolve();
    }
}