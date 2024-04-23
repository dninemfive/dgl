namespace d9.dgl;
public delegate CellDelta Rule(Cell previous, IEnumerable<Cell> neighbors);
public static class Rules
{
    public static IReadOnlyCollection<Rule> All => _all;
    private static readonly List<Rule> _all = new()
    {
        
    };
    public CellDelta GrassGrow(Cell previous, IEnumerable<Cell> neighbors)
    {
        if(previous.G + neighbors.Population(Component.G) > 0)
    }
}