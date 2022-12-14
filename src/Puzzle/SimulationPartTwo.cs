using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using static AdventOfCode2022.TryHelpers;

namespace AdventOfCode2022;

internal sealed class SimulationPartTwo
{
    private static readonly string[] s_arrowSeparators = { " -> " };

    private readonly HashSet<Point> _rocks;
    private readonly IReadOnlySet<Point> _sandUnits;

    private SimulationPartTwo(HashSet<Point> rocks, IReadOnlySet<Point> sandUnits, int bottom)
    {
        _rocks = rocks;
        _sandUnits = sandUnits;
        Bottom = bottom;
    }

    private int Bottom { get; }

    internal IReadOnlySet<Point> Rocks => _rocks;

    internal bool TrySimulateSandUnit(Point initialPosition, out Point finalPosition)
    {
        if (!IsEmpty(initialPosition))
            return None(out finalPosition);

        Point currentPosition = initialPosition;
        while (true)
        {
            if (currentPosition.Y == Bottom - 1)
                return Some(currentPosition, out finalPosition);

            Point bottomCandidate = currentPosition + new Size(0, 1);
            if (IsEmpty(bottomCandidate))
            {
                currentPosition = bottomCandidate;
                continue;
            }

            Point leftCandidate = currentPosition + new Size(-1, 1);
            if (IsEmpty(leftCandidate))
            {
                currentPosition = leftCandidate;
                continue;
            }

            Point rightCandidate = currentPosition + new Size(1, 1);
            if (IsEmpty(rightCandidate))
            {
                currentPosition = rightCandidate;
                continue;
            }

            return Some(currentPosition, out finalPosition);
        }
    }

    internal static SimulationPartTwo Create(IReadOnlyList<string> lines, IReadOnlySet<Point> sandUnits)
    {
        ArgumentNullException.ThrowIfNull(sandUnits);

        HashSet<Point> rocks = new(lines.Count);
        foreach (string line in lines)
        {
            string[] parts = line.Split(s_arrowSeparators, StringSplitOptions.RemoveEmptyEntries);
            Point[] points = parts.Select(ParsePoint).ToArray();
            IEnumerable<(Point, Point)> pairs = points.Zip(points.Skip(1));
            foreach ((Point first, Point second) in pairs)
            {
                int minX = Math.Min(first.X, second.X);
                int maxX = Math.Max(first.X, second.X);
                int minY = Math.Min(first.Y, second.Y);
                int maxY = Math.Max(first.Y, second.Y);
                for (int x = minX; x <= maxX; ++x)
                {
                    for (int y = minY; y <= maxY; ++y)
                        rocks.Add(new(x, y));
                }
            }
        }

        int bottom = rocks.Max(rock => rock.Y);
        return new(rocks, sandUnits, bottom + 2);
    }

    private static Point ParsePoint(string line)
    {
        string[] parts = line.Split(',', StringSplitOptions.TrimEntries);
        return new(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    private bool IsEmpty(Point position) => !_rocks.Contains(position) && !_sandUnits.Contains(position);
}
