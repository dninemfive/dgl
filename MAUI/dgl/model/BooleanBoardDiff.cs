namespace d9.dgl;
public readonly struct BooleanBoardDiff(IEnumerable<(int x, int y)> flips)
{
    public readonly IReadOnlySet<(int x, int y)> Flips => flips.ToHashSet();
    public static Board<bool> operator+(Board<bool> original, BooleanBoardDiff diff)
    {
        bool[,] array = original;
        foreach ((int x, int y) in diff.Flips)
            array[x, y] = !array[x, y];
        return array;
    }
}
