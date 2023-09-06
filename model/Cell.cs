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
        foreach (Component component in Components.All)
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