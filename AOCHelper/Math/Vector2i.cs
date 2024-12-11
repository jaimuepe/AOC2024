namespace AOCHelper.Math;

public struct Vector2i : IEquatable<Vector2i>
{
    public static readonly Vector2i Up = new(0, 1);
    public static readonly Vector2i Right = new(1, 0);
    public static readonly Vector2i Down = new(0, -1);
    public static readonly Vector2i Left = new(-1, 0);

    public int X { get; set; }

    public int Y { get; set; }

    public Vector2i()
    {
    }

    public Vector2i(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vector2i operator +(Vector2i a) => a;

    public static Vector2i operator -(Vector2i a) => new(-a.X, -a.Y);

    public static Vector2i operator +(Vector2i a, Vector2i b) => new(a.X + b.X, a.Y + b.Y);

    public static Vector2i operator -(Vector2i a, Vector2i b) => a + -b;

    public static bool operator ==(Vector2i a, Vector2i b) => a.Equals(b);

    public static bool operator !=(Vector2i a, Vector2i b) => !(a == b);

    public static Vector2i RotateRight(Vector2i vec)
    {
        // (1, 0) -> (0, -1)
        // (0, 1) -> (1, 0)
        // (-1, 0) -> (0, 1)
        // (0, -1) -> (-1, 0)
        return new Vector2i(vec.Y, -vec.X);
    }
    
    public static Vector2i RotateLeft(Vector2i vec)
    {
        // (1, 0) -> (0, 1)
        // (0, 1) -> (-1, 0)
        // (-1, 0) -> (0, -1)
        // (0, -1) -> (1, 0)
        return new Vector2i(-vec.Y, vec.X);
    }
    
    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }

    public bool Equals(Vector2i other)
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