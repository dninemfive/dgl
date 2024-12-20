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
}