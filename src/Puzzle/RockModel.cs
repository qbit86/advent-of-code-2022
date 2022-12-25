using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2022;

internal sealed class RockModel
{
    private RockModel(IReadOnlyCollection<Point> blocks, Point minBound, Point maxBound)
    {
        Blocks = blocks;
        MinBound = minBound;
        MaxBound = maxBound;
    }

    internal IReadOnlyCollection<Point> Blocks { get; }
    internal Point MinBound { get; }
    internal Point MaxBound { get; }

    internal static RockModel Create(IReadOnlyCollection<Point> blocks)
    {
        Point min = blocks.Aggregate(Min);
        Point max = blocks.Aggregate(Max);

        return new(blocks, min, max);

        static Point Min(Point left, Point right)
        {
            int x = Math.Min(left.X, right.X);
            int y = Math.Min(left.Y, right.Y);
            return new(x, y);
        }

        static Point Max(Point left, Point right)
        {
            int x = Math.Max(left.X, right.X);
            int y = Math.Max(left.Y, right.Y);
            return new(x, y);
        }
    }
}
