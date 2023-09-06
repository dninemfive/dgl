using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d9.dgl;
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
    public static IEnumerable<byte> InBytesLsb(this int n)
    {
        if (n < 0)
        {
            foreach (byte b in (-n).InBytesLsb())
                yield return b;
        }
        while(n > 0)
        {
            yield return (byte)(n % 255);
            n /= 255;
        }
    }
    public static IEnumerable<byte> InBytesMsb(this int n) => n.InBytesLsb().Reverse();
}