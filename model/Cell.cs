using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.dgl;
public readonly struct Cell
{
    private readonly byte[] _rgb = new byte[3];
    public byte R => _rgb[0];
    public byte G => _rgb[1];
    public byte B => _rgb[2];
    public Cell(byte r, byte g, byte b)
    {
        _rgb[0] = r;
        _rgb[1] = g;
        _rgb[2] = b;
    }
    public Cell(Cell previous, IEnumerable<Cell> neighbors)
    {
        CellDelta delta = new();
        foreach (Rule rule in Rules.All)
            delta += rule(previous, neighbors);
    }
    public byte this[Component c] => _rgb[(int)c];
    public int Total => _rgb.Select(x => (int)x).Sum();
    public static Cell operator +(Cell cell, CellDelta delta)
        => new(cell.R.Add(delta.R), cell.G.Add(delta.G), cell.B.Add(delta.B));
}
public readonly struct CellDelta
{
    public readonly int R, G, B;
    public CellDelta() : this(0, 0, 0) { }
    public CellDelta(int r, int g, int b)
    {
        R = r;
        G = g;
        B = b;
    }
    public static CellDelta operator+(CellDelta a, CellDelta b) => new(a.R + b.R, a.G + b.G, a.B + b.B);
    public static implicit operator CellDelta((int r, int g, int b) t) => new(t.r, t.g, t.b);
}