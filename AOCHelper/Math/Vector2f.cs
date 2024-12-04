namespace AOCHelper.Math;

public struct Vector2f
{
    public float X { get; set; }
        
    public float Y { get; set; }
    
    public Vector2f()
    {
    }

    public Vector2f(float x, float y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }
}