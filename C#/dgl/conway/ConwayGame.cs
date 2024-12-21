using d9.dgl.framework;

namespace d9.dgl.conway;
public class ConwayGame
{
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

    public async Task EvolveAsync()
    {
        await foreach (Point _ in EvolveLiveCellsAsync())
            ;
    }
    public async IAsyncEnumerable<Point> EvolveLiveCellsAsync() {
        // https://stackoverflow.com/a/1460660
        ConwayCell[,] nextState = CurrentState;
        await foreach (Point p in CurrentState.EvolveAsync())
        {
            (int x, int y) = p;
            nextState[x, y] = !nextState[x, y];
            if (nextState[x, y])
                yield return p;
        }
        CurrentState = nextState;
    }
}