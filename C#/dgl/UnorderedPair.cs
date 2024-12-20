using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace d9.dgl;

public readonly struct UnorderedPair<T>(T a, T b)
    where T : struct, IEqualityOperators<T, T, bool>
{
    public readonly T A = a, B = b;
    public override int GetHashCode()
        => HashCode.Combine(A, B);
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if(obj is UnorderedPair<T> other)
        {
            return (other.A == A && other.B == B) || (other.B == A && other.A == B);
        }
        return false;
    }
    public static bool operator ==(UnorderedPair<T> left, UnorderedPair<T> right)
        => left.Equals(right);
    public static bool operator !=(UnorderedPair<T> left, UnorderedPair<T> right)
        => !(left == right);
    public void Deconstruct(out T a, out T b)
    {
        a = A;
        b = B;
    }
    public static implicit operator UnorderedPair<T>((T a, T b) tuple) 
        => new(tuple.a, tuple.b);
}
