using d9.aoc.core;

namespace d9.dgl.framework;
public static class Extensions
{
    public static IEnumerable<Point<int>> AllPoints<T>(this T[,] array)
    {
        for (int x = 0; x < array.Width(); x++)
            for (int y = 0; y < array.Height(); y++)
                yield return (x, y);
    } 
}