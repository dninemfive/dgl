using d9.aoc.core;
using System.Numerics;

namespace d9.dgl;
internal static class Extensions
{
    internal static ISet<(F distance, UnorderedPair<Cell<C>> pair)> AllPairsIn<C, F>(this Grid<Cell<C>> grid)
        where C : struct, INumber<C>
        where F : IRootFunctions<F>
    {
        HashSet<(F distance, UnorderedPair<Cell<C>> pair)> result = [];
        foreach(Point<int> a in grid.AllPoints)
            foreach(Point<int> b in grid.AllPoints)
            {
                if (a == b)
                    continue;
                result.Add((F.Sqrt(F.CreateChecked((a - b).SquareMagnitude)), (grid[a], grid[b])));
            }
        return result;
    }
}
