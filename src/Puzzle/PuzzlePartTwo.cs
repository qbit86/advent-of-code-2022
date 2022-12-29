using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo : IPuzzle<long>
{
    private readonly Size _area;

    private PuzzlePartTwo(Size area) => _area = area;

    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    public async Task<Point> FindSingleBeaconAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return FindSingleBeacon(lines);
    }

    public static PuzzlePartTwo Create(int boundInclusive) => new(new(boundInclusive + 1, boundInclusive + 1));

    public static bool TryIntersect(
        Point leftPosition, Size leftDirection, Point rightPosition, Size rightDirection, out Point result)
    {
        // https://cp-algorithms.com/geometry/basic-geometry.html#line-intersection
        // return a1 + cross(a2 - a1, d2) / cross(d1, d2) * d1;
        int denominator = leftDirection.Cross(rightDirection);
        if (denominator == 0)
        {
            result = default;
            return false;
        }

        Size directionFromLeftToRight = rightPosition.Subtract(leftPosition);
        int numerator = directionFromLeftToRight.Cross(rightDirection);
        int t = numerator / denominator;
        result = leftPosition + t * leftDirection;
        return true;
    }

    private long SolveCore(IReadOnlyList<string> lines)
    {
        Point beacon = FindSingleBeacon(lines);
        return beacon.X * 4000000L + beacon.Y;
    }

    private Point FindSingleBeacon(IReadOnlyList<string> lines)
    {
        SensorRecord[] sensors = lines.Select(SensorRecord.Parse).ToArray();
        RadiusRecord[] radii = sensors.Select(Extensions.Radius).ToArray();
        int count = radii.Length;
        List<Point> intersectionCandidates = new(count * (count - 1) * 2);
        for (int i = 0; i < count; ++i)
        {
            for (int j = i + 1; j < count; ++j)
                PopulateIntersectionCandidates(radii[i], radii[j], intersectionCandidates);
        }

        var beaconCandidates = intersectionCandidates.SelectMany(SelectNeighbors)
            .Where(IsInsideArea)
            .Distinct()
            .Where(IsOutsideRadii)
            .ToList();

        return beaconCandidates.Single();

        bool IsOutsideRadii(Point beaconCandidate)
        {
            return !radii.Any(it => it.Contains(beaconCandidate));
        }
    }

    private static void PopulateIntersectionCandidates(
        RadiusRecord left, RadiusRecord right, ICollection<Point> intersectionCandidates)
    {
        {
            Point leftPosition = left.Position + new Size(0, -left.Radius);
            Size leftDirection = new(-1, 1);
            Point rightPosition = right.Position + new Size(0, -right.Radius);
            Size rightDirection = new(1, 1);
            intersectionCandidates.Add(Intersect(leftPosition, leftDirection, rightPosition, rightDirection));
        }
        {
            Point leftPosition = left.Position + new Size(0, -left.Radius);
            Size leftDirection = new(1, 1);
            Point rightPosition = right.Position + new Size(0, -right.Radius);
            Size rightDirection = new(-1, 1);
            intersectionCandidates.Add(Intersect(leftPosition, leftDirection, rightPosition, rightDirection));
        }
        {
            Point leftPosition = left.Position + new Size(0, left.Radius);
            Size leftDirection = new(-1, -1);
            Point rightPosition = right.Position + new Size(0, right.Radius);
            Size rightDirection = new(1, -1);
            intersectionCandidates.Add(Intersect(leftPosition, leftDirection, rightPosition, rightDirection));
        }
        {
            Point leftPosition = left.Position + new Size(0, left.Radius);
            Size leftDirection = new(1, -1);
            Point rightPosition = right.Position + new Size(0, right.Radius);
            Size rightDirection = new(-1, -1);
            intersectionCandidates.Add(Intersect(leftPosition, leftDirection, rightPosition, rightDirection));
        }
    }

    private static Point Intersect(Point leftPosition, Size leftDirection, Point rightPosition, Size rightDirection)
    {
        if (TryIntersect(leftPosition, leftDirection, rightPosition, rightDirection, out Point result))
            return result;
        throw new UnreachableException();
    }

    private static IEnumerable<Point> SelectNeighbors(Point position) => new[]
    {
        position + new Size(1, 0),
        position + new Size(0, 1),
        position + new Size(-1, 0),
        position + new Size(0, -1)
    };

    private bool IsInsideArea(Point beaconCandidate) =>
        unchecked((uint)beaconCandidate.X < (uint)_area.Width && (uint)beaconCandidate.Y < (uint)_area.Height);
}
