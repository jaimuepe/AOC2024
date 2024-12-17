namespace AOCHelper.Math;

public struct Vector2L : IEquatable<Vector2L>
{
    public static readonly Vector2L Up = new(0L, 1L);
    public static readonly Vector2L Right = new(1L, 0L);
    public static readonly Vector2L Down = new(0L, -1L);
    public static readonly Vector2L Left = new(-1L, 0L);

    public long X { get; set; }

    public long Y { get; set; }

    public Vector2L()
    {
    }

    public Vector2L(long x, long y)
    {
        X = x;
        Y = y;
    }
    
    public static implicit operator Vector2L(Vector2I d) => new (d.X, d.Y);

    public static Vector2L operator *(Vector2L a, int n) => new(a.X * n, a.Y * n);

    public static Vector2L operator *(int n, Vector2L a) => new(a.X * n, a.Y * n);

    public static Vector2L operator +(long n, Vector2L a) => new(a.X + n, a.Y + n);
    
    public static Vector2L operator +(Vector2L a, long n) => new(a.X + n, a.Y + n);
    
    public static Vector2L operator +(Vector2L a) => a;

    public static Vector2L operator -(Vector2L a) => new(-a.X, -a.Y);

    public static Vector2L operator +(Vector2L a, Vector2L b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector2L operator -(Vector2L a, Vector2L b) => a + -b;

    public static bool operator ==(Vector2L a, Vector2L b) => a.Equals(b);

    public static bool operator !=(Vector2L a, Vector2L b) => !(a == b);

    public static Vector2L RotateRight(Vector2L vec)
    {
        // (1, 0) -> (0, -1)
        // (0, 1) -> (1, 0)
        // (-1, 0) -> (0, 1)
        // (0, -1) -> (-1, 0)
        return new Vector2L(vec.Y, -vec.X);
    }

    public static Vector2L RotateLeft(Vector2L vec)
    {
        // (1, 0) -> (0, 1)
        // (0, 1) -> (-1, 0)
        // (-1, 0) -> (0, -1)
        // (0, -1) -> (1, 0)
        return new Vector2L(-vec.Y, vec.X);
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector2L other && Equals(other);
    }
    
    public bool Equals(Vector2L other)
    {
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public void Deconstruct(out long x, out long y)
    {
        x = X;
        y = Y;
    }
}