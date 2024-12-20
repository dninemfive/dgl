using d9.aoc.core;
using System.Numerics;

namespace d9.dgl;
/// <typeparam name="C">The type of each cell <u>c</u>omponent.</typeparam>
internal class Universe<C>
    where C : struct, INumber<C>
{
    private readonly List<Grid<Cell<C>>> _states = [];
}
