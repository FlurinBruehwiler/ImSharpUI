﻿using Flamui.Layouting;
using Flamui.UiElements;
using SkiaSharp;

namespace Flamui;

public record struct Quadrant(float Left, float Right, float Top, float Bottom)
{
    public float SumInDirection(Dir dir)
    {
        return dir switch
        {
            Dir.Horizontal => Left + Right,
            Dir.Vertical => Top + Bottom,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    public float EndOfDirection(Dir dir)
    {
        return dir switch
        {
            Dir.Horizontal => Right,
            Dir.Vertical => Bottom,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    public float StartOfDirection(Dir dir)
    {
        return dir switch
        {
            Dir.Horizontal => Left,
            Dir.Vertical => Top,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }
}
public record struct AbsolutePosition(float? Left, float? Right, float? Top, float? Bottom);

public enum MAlign
{
    FlexStart = 0, //Default
    FlexEnd,
    Center,
    FlexBetween
}

public enum Dir
{
    Vertical = 0, //Default
    Horizontal,
}

public enum XAlign
{
    FlexStart = 0, //Default
    FlexEnd,
    Center
}

public enum SizeKind
{
    Percentage = 0, //Default
    Pixel,
    Shrink
}


public static class Extensions
{
    public static bool IsFlexible(this UiElement uiElement, out FlexibleChildConfig config)
    {
        config = new FlexibleChildConfig();

        if (uiElement.FlexibleChildConfig is null)
        {
            return false;
        }

        config = uiElement.FlexibleChildConfig.Value;

        return true;
    }

    public static Dir Other(this Dir dir)
    {
        return dir == Dir.Horizontal ? Dir.Vertical : Dir.Horizontal;
    }


    public static float GetMain(this Dir dir, float width, float height)
    {
        return dir switch
        {
            Dir.Horizontal => width,
            Dir.Vertical => height,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    public static float Cross(this Dir dir, float width, float height)
    {
        return dir switch
        {
            Dir.Horizontal => height,
            Dir.Vertical => width,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }
}

public readonly record struct ColorDefinition(byte Red, byte Green, byte Blue, byte Alpha = 255)
{
    public SKColor ToSkColor()
    {
        return new SKColor(Red, Green, Blue, Alpha);
    }

    public static ColorDefinition operator /(ColorDefinition original, byte opacity)
    {
        return original with
        {
            Alpha = (byte)(original.Alpha / opacity)
        };
    }
}

