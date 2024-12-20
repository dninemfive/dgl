using d9.aoc.core;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace d9.dgl;
internal readonly struct Cell<T>(T r, T g, T b, Point<int> coordinate)
    : IEqualityOperators<Cell<T>, Cell<T>, bool>
    where T : struct, INumber<T>
{
    public readonly T R = r, G = g, B = b;
    public readonly Point<int> Coordinate = coordinate;
    public override string ToString()
        => $"Cell({R}, {G}, {B})@{Coordinate}";
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is Cell<T> other && other.Coordinate == Coordinate;
    public static bool operator ==(Cell<T> left, Cell<T> right)
        => left.Equals(right);
    public static bool operator !=(Cell<T> left, Cell<T> right)
        => !(left == right);
    public override int GetHashCode()
        => HashCode.Combine(Coordinate);
}
