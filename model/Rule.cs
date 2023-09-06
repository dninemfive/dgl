using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.dgl;
public delegate int Rule(Cell previous, IEnumerable<Cell> neighbors);
public static class Rules
{
    public static IEnumerable<Rule> All
    {
        get
        {
            foreach(IEnumerable<Rule> set in _dict.Values)
            {
                foreach (Rule rule in set)
                    yield return rule;
            }
        }
    }
    public static IEnumerable<Rule> For(Component c) => _dict[c];
    private static readonly Dictionary<Component, List<Rule>> _dict = new()
    {
        {
            // models traditional conway, adapted to bytes instead of bools
            Component.R, new()
            {
                // rule 1: Any live (R > 1) cell with fewer than two live neighbors (64 pop) dies (decrements), as if by underpopulation
                (previous, neighbors) => previous.R > 0 && neighbors.Population() < 64 * 2 ? -1 : 0,
                // rule 2: Any live (R > 1) cell with two or three live neighbors lives on to the next generation
                // no rule required bc by default there's no change
                // rule 3: Any live (R > 1) cell with more than three live neighbors (92 pop) dies, as if by overpopulation
                (previous, neighbors) => previous.R > 0 && neighbors.Population(Component.R) > 32 * 4 ? -1 : 0,
                // rule 4: Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction
                (previous, neighbors) => previous.R < 255 && neighbors.Population(Component.R) is > 32 * 3 and < 32 * 4 ? 1 : 0
            }
        },
        {
            // just grass growing
            Component.G, new()
            {
                (previous, _) => previous.G < 127 ? 1 : 0
            }
        },
        {
            // herbivores or smth
            // todo: make it so the grass can actually decrease
            Component.B, new()
            {
                (previous, neighbors) => previous.B > 0 && neighbors.Population() > 127 ? -1 : 0,
                (previous, neighbors) => neighbors.Population(Component.B) < 127 ? 1 : 0
            }
        }
    };
}