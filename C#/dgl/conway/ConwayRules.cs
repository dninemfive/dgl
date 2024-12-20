namespace d9.dgl.conway;
public delegate ConwayCell? ConwayRule(ConwayCell cell, int neighborCount);
public static class ConwayRules
{
    public static bool IsAlive(this ConwayCell cell)
        => cell;
    public static readonly IEnumerable<ConwayRule> All = [
        DieByUnderpopulation,
        Live,
        DieByOverpopulation,
        Reproduce
    ];
    public static ConwayCell? DieByUnderpopulation(ConwayCell cell, int neighborCount)
        => cell.IsAlive() && (neighborCount is 0 or 1) ? false : null;
    public static ConwayCell? Live(ConwayCell cell, int neighborCount)
        => cell.IsAlive() && (neighborCount is 2 or 3) ? true : null;
    public static ConwayCell? DieByOverpopulation(ConwayCell cell, int neighborCount)
        => cell.IsAlive() && (neighborCount > 3) ? false : null;
    public static ConwayCell? Reproduce(ConwayCell cell, int neighborCount)
        => !cell.IsAlive() && neighborCount is 3 ? true : null;
}