using Flamui.UiElements;

namespace Flamui.Drawing;

public static class FontShaping
{
    public static (float start, float end) GetPositionOfChar(ScaledFont scaledFont, ReadOnlySpan<char> singleLine, int index)
    {
        float xCoord = 0;

        if (index < 0 || index > singleLine.Length)
            return default;

        for (var i = 0; i < index + 1; i++)
        {
            var c = singleLine[i];

            var start = xCoord;

            xCoord += scaledFont.GetCharWidth(c);

            if (index == i)
            {
                return (start, xCoord);
            }
        }

        return default;
    }

    /// <returns>The width of the text</returns>
    public static float MeasureText(ScaledFont scaledFont, ArenaString singleLine)
    {
        var width = 0f;
        for (var i = 0; i < singleLine.Length; i++)
        {
            var c = singleLine[i];
            width += scaledFont.GetCharWidth(c);
        }

        return width;
    }

    /// <summary>
    /// Performs a horizontal hit test against a piece of text.
    /// </summary>
    /// <param name="scaledFont">The font to use</param>
    /// <param name="singleLine">A piece of text, that lives on a single line</param>
    /// <param name="pos">The position relative to the left of the line</param>
    /// <returns>The index of the char that is under, -1 the pos was outside the text <see cref="pos"/></returns>
    public static int HitTest(ScaledFont scaledFont, ReadOnlySpan<char> singleLine, float pos)
    {
        float xCoord = 0;

        if (pos < 0)
            return -1;

        for (var i = 0; i < singleLine.Length; i++)
        {
            var c = singleLine[i];

            xCoord += scaledFont.GetCharWidth(c);

            if (pos < xCoord)
                return i;
        }

        return -1;
    }

    //rule: preferably only ever the start of a new word can go onto the next line,
    //so we make a new line, as soon as the next word + following whitespace doesn't fit on the current line
    //if we can't even fit a single word on a line, we have to start to split in the middle of the word!
    public static TextLayoutInfo LayoutText(ScaledFont scaledFont, ArenaString text, float maxWidth, TextAlign horizontalAlignement, bool multilineAllowed)
    {
        List<Line> lines = [];
        float widthOfLongestLine = 0;

        int currentBlockStart = 0;
        float currentBlockWidth = 0;

        int currentLineStart = 0;
        float currentLineWidth = 0;

        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];

            if (i != 0 && char.IsWhiteSpace(text[i - 1]) && !char.IsWhiteSpace(c))
            {
                currentBlockStart = i;
                currentBlockWidth = 0;
            }

            if (c is '\n' or '\r' && multilineAllowed)
            {
                if (c == '\r' && text.Length > i + 1 && text[i + 1] == '\n')
                    i++;

                //add new line
                AddLine(i, text);
                currentLineWidth = 0;
                currentLineStart = i + 1;
                currentBlockStart = i + 1;
                continue;
            }

            var charWidth = scaledFont.GetCharWidth(c);

            currentLineWidth += charWidth;
            currentBlockWidth += charWidth;


            if (currentLineWidth > maxWidth && multilineAllowed)
            {
                if (currentLineStart == currentBlockStart) //not even a single word fits onto the line
                {
                    AddLine(i, text);
                    currentLineStart = i;
                    currentBlockStart = i;
                    currentLineWidth = charWidth;
                    currentBlockWidth = charWidth;
                }
                else
                {
                    //add new line
                    AddLine(currentBlockStart, text);
                    currentLineWidth = currentBlockWidth;
                    currentLineStart = currentBlockStart;
                }
            }
        }

        AddLine(text.Length, text);

        var yCoord = 0f;
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];

            var bounds = line.Bounds with
            {
                X = horizontalAlignement switch
                {
                    TextAlign.Start => 0,
                    TextAlign.Center => (maxWidth - line.Bounds.W) / 2,
                    TextAlign.End => maxWidth - line.Bounds.W,
                    _ => throw new ArgumentOutOfRangeException()
                },
                Y = yCoord,
            };

            lines[i] = line with
            {
                Bounds = bounds
            };

            yCoord += scaledFont.GetHeight() + scaledFont.LineGap;
        }

        return new TextLayoutInfo
        {
            Lines = lines.ToArray(),
            MaxWidth = widthOfLongestLine,
            TotalHeight = lines.Count * scaledFont.GetHeight() + lines.Count - 1 * (scaledFont.LineGap),
        };

        void AddLine(int endIndex, ArenaString t)
        {
            var r = new Range(new Index(currentLineStart), new Index(endIndex));
            var width = MeasureText(scaledFont, t[r]);
            widthOfLongestLine = Math.Max(widthOfLongestLine, width);
            lines.Add(new Line
            {
                TextContent = text[r],
                Bounds = new Bounds
                {
                    W = width,
                    H = scaledFont.GetHeight(),
                    X = 0, //will be set later
                    Y = 0
                }
            });
        }
    }
}

public struct TextLayoutInfo
{
    public Line[] Lines;
    public float MaxWidth;
    public float TotalHeight;
}

public struct Line
{
    public Bounds Bounds;
    public ArenaString TextContent;
    public Slice<float> CharOffsets; // the start of each char on the x axis
}