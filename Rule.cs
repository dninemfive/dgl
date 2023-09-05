using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dgl;
public enum Component { R = 0, G = 1, B = 2 }
public static class Components
{
    public static IEnumerable<Component> All => Enum.GetValues<Component>();
}
public readonly struct Cell
{
    private readonly byte[] _rgb;
    public byte R => _rgb[0];
    public byte G => _rgb[1];
    public byte B => _rgb[2];
    public Cell(byte r, byte g, byte b)
    {
        _rgb = new byte[3];
        _rgb[0] = r;
        _rgb[1] = g;
        _rgb[2] = b;
    }
    public Cell(Cell previous, IEnumerable<Cell> neighbors)
    {
        _rgb = new byte[3];
        foreach(Component component in Components.All)
        {
            byte val = previous[component];
            foreach (Rule rule in Rules.For(component))
                val = val.Add(rule(previous, neighbors));
            _rgb[(int)component] = val;
        }
    }
    public byte this[Component c] => _rgb[(int)c];
    public int Total => _rgb.Select(x => (int)x).Sum();
}
public readonly ref struct Board
{
    private readonly Cell[,] _cells;
    public int Width => _cells.GetLength(0);
    public int Height => _cells.GetLength(1);
    public Board(int width, int height)
    {
        _cells = new Cell[width, height];
    }
    public Board(Board previous) : this(previous.Width, previous.Height)
    {
        for(int x = 0; x < Width; x++)
        {
            for(int y = 0; y < Height; y++)
            {
                _cells[x, y] = new(previous[x, y], previous.NeighborsOf(x, y));
            }
        }
    }
    public bool InBounds(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;
    public Cell this[int x, int y] => InBounds(x, y) ? this[x, y] : throw new Exception($"{x} and/or {y} is not in the range of board {this}!");
    public IEnumerable<Cell> NeighborsOf(int x, int y)
    {
        for(int i = -1; i <= 1; i++)
        {
            for(int j =  -1; j <= 1; j++)
            {
                int xx = x + i, yy = y + j;
                if (i == 0 && j == 0 || !InBounds(xx, yy))
                    continue;
                yield return _cells[xx, yy];
            }
        }
    }
}
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
public static class Extensions 
{
    public static int Population(this IEnumerable<Cell> cells, Component? c = null)
        => c is null ? cells.Select(x => x.Total).Sum() : cells.Select(x => (int)x[c.Value]).Sum();
    public static byte Add(this byte b, int i) => (byte)(b + i).Clamp();
    public static int Clamp(this int i) => i switch
    {
        < 0 => 0,
        > 255 => 255,
        _ => i
    };
}