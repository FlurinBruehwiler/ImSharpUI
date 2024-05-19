﻿using Flamui.Layouting;
using EnumMAlign = Flamui.MAlign;

namespace Flamui.UiElements;

public static class FlexPositionCalculator
{
    public static BoxSize ComputePosition(List<UiElement> children, BoxSize size, FlexContainerInfo info)
    {
        if(children.Count == 0)
            return size;

        switch (info.MainAlignment)
        {
            case EnumMAlign.FlexStart:
                return CalculateFlexStart(children, size, info);
            // case EnumMAlign.FlexEnd:
            //     return CalculateFlexEnd(children, dir);
            // case EnumMAlign.SpaceBetween:
            //     return RenderSpaceBetween(children, dir);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static BoxSize CalculateFlexStart(List<UiElement> children, BoxSize size, FlexContainerInfo info)
    {
        var mainOffset = 0f;
        var crossSize = 0f;

        foreach (var child in children)
        {
            SetPosition(mainOffset, child, size, info);
            mainOffset += child.BoxSize.GetMainAxis(info.Direction) + info.Gap;

            var childCrossSize = child.BoxSize.GetCrossAxis(info.Direction);
            if (childCrossSize > crossSize)
                crossSize += childCrossSize;
        }

        var mainSize = mainOffset - info.Gap;

        return BoxSize.FromDirection(info.Direction, mainSize + info.PaddingSizeMain(), crossSize + info.PaddingSizeCross());
    }

    // private static BoxSize CalculateFlexEnd(List<ILayoutable> children, Dir dir)
    // {
    //     var mainOffset = RemainingMainAxisSize();
    //
    //     foreach (var child in Children)
    //     {
    //         if (child is UiContainer { PAbsolute: true } divChild)
    //         {
    //             PositionAbsoluteItem(divChild);
    //             continue;
    //         }
    //
    //         SetPosition(mainOffset, child);
    //         mainOffset += GetItemMainAxisLength(child) + PGap;
    //     }
    //
    //     return new Size();
    // }
    //
    // private Size RenderSpaceBetween()
    // {
    //     var totalRemaining = RemainingMainAxisSize();
    //     var space = totalRemaining / (Children.Count - 1);
    //
    //     var mainOffset = 0f;
    //
    //     foreach (var child in Children)
    //     {
    //         if (child is UiContainer { PAbsolute: true } divChild)
    //         {
    //             PositionAbsoluteItem(divChild);
    //             continue;
    //         }
    //
    //         SetPosition(mainOffset, child);
    //         mainOffset += GetItemMainAxisLength(child) + space + PGap;
    //     }
    //
    //     return new Size();
    // }

    private static void SetPosition(float mainOffset, UiElement item, BoxSize size, FlexContainerInfo info)
    {
        var point = Point.FromDirection(info.Direction, mainOffset,
            GetCrossAxisOffset(item, info.CrossAlignment, size, info.Direction));

        item.ParentData = item.ParentData with
        {
            Position = new Point(point.X + info.Padding.Left, point.Y + info.Padding.Top)
        };
    }

    private static float GetCrossAxisOffset(UiElement item, XAlign xAlign, BoxSize size, Dir dir)
    {
        return xAlign switch
        {
            XAlign.FlexStart => 0,
            XAlign.FlexEnd => size.GetCrossAxis(dir) - item.BoxSize.GetCrossAxis(dir),
            XAlign.Center => size.GetCrossAxis(dir) / 2 - item.BoxSize.GetCrossAxis(dir) / 2,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}