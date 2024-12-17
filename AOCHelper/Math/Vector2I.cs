namespace AOCHelper.Math;

public struct Vector2I : IEquatable<Vector2I>
{
    public static readonly Vector2I Up = new(0, 1);
    public static readonly Vector2I Right = new(1, 0);
    public static readonly Vector2I Down = new(0, -1);
    public static readonly Vector2I Left = new(-1, 0);

    public int X { get; set; }

    public int Y { get; set; }

    public Vector2I()
    {
    }

    public Vector2I(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vector2I operator *(Vector2I a, int n) => new(a.X * n, a.Y * n);

    public static Vector2I operator *(int n, Vector2I a) => new(a.X * n, a.Y * n);

    public static Vector2I operator +(Vector2I a) => a;

    public static Vector2I operator -(Vector2I a) => new(-a.X, -a.Y);

    public static Vector2I operator +(Vector2I a, Vector2I b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector2I operator -(Vector2I a, Vector2I b) => a + -b;

    public static bool operator ==(Vector2I a, Vector2I b) => a.Equals(b);

    public static bool operator !=(Vector2I a, Vector2I b) => !(a == b);

    public static Vector2I RotateRight(Vector2I vec)
    {
        // (1, 0) -> (0, -1)
        // (0, 1) -> (1, 0)
        // (-1, 0) -> (0, 1)
        // (0, -1) -> (-1, 0)
        return new Vector2I(vec.Y, -vec.X);
    }

    public static Vector2I RotateLeft(Vector2I vec)
    {
        // (1, 0) -> (0, 1)
        // (0, 1) -> (-1, 0)
        // (-1, 0) -> (0, -1)
        // (0, -1) -> (1, 0)
        return new Vector2I(-vec.Y, vec.X);
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector2I other && Equals(other);
    }
    
    public bool Equals(Vector2I other)
    {
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }
}