using System.Drawing;
using System.Numerics;

namespace Flamui.Drawing;

public struct TextPos
{
    public int Line;
    public int Char;
}

public struct Vertex
{
    public Vector2 Position;
    public Vector2 UV;
    public float BezierFillType;
    public Color Color;
    public TextureType TextureType;

    public Vertex(Vector2 position, Vector2 uv, Color color)
    {
        Position = position;
        UV = uv;
        Color = color;
    }
}

public struct Rect
{
    public Rect(float x, float y, float width, float height)
    {
        Pos = new Vector2(x, y);
        Size = new Vector2(width, height);
    }

    public Vector2 Pos;
    public Vector2 Size;
}

public struct BezierCurve
{
    public BezierCurve(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
    }

    public Vector2 P1;
    public Vector2 P2;
    public Vector2 P3;
}