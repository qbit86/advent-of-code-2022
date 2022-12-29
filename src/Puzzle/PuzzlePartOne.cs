using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartOne : IPuzzle<long>
{
    private readonly int _rowIndexOfInterest;

    public PuzzlePartOne(int rowIndexOfInterest) => _rowIndexOfInterest = rowIndexOfInterest;

    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    private long SolveCore(IReadOnlyList<string> lines)
    {
        SensorRecord[] sensors = lines.Select(SensorRecord.Parse).ToArray();
        RadiusRecord[] radii = sensors.Select(Extensions.Radius).ToArray();
        RangeRecord[] ranges = radii.Select(SelectRange).ToArray();
        RangeRecord[] orderedRanges = ranges.Where(it => it.Length > 0).OrderBy(it => it.Start).ToArray();
        List<RangeRecord> disjointRanges = new();
        _ = ComputeDisjointRanges(orderedRanges[0], orderedRanges.AsSpan()[1..], disjointRanges);
        Point[] knownBeaconPositions = sensors.Select(it => it.BeaconPosition).Distinct().ToArray();
        Point[] beaconPositionsOnRowOfInterest =
            knownBeaconPositions.Where(it => it.Y == _rowIndexOfInterest).ToArray();
        long result = 0;
        foreach (RangeRecord disjointRange in disjointRanges)
        {
            int beaconCount = beaconPositionsOnRowOfInterest.Count(it => disjointRange.Contains(it.X));
            result += disjointRange.Length - beaconCount;
        }

        return result;

        static IReadOnlyList<RangeRecord> ComputeDisjointRanges(
            RangeRecord head, ReadOnlySpan<RangeRecord> tail, List<RangeRecord> disjointRanges)
        {
            if (tail.Length == 0)
            {
                disjointRanges.Add(head);
                return disjointRanges;
            }

            RangeRecord next = tail[0];
            tail = tail[1..];

            if (head.Length == 0)
                return ComputeDisjointRanges(next, tail, disjointRanges);

            if (head.TryJoin(next, out RangeRecord result))
                return ComputeDisjointRanges(result, tail, disjointRanges);

            disjointRanges.Add(head);
            return ComputeDisjointRanges(next, tail, disjointRanges);
        }

        RangeRecord SelectRange(RadiusRecord radiusRecord)
        {
            int distance = Math.Abs(radiusRecord.Position.Y - _rowIndexOfInterest);
            int range = radiusRecord.Radius - distance;
            int x = radiusRecord.Position.X;
            if (range < 0)
                return new(x, x);

            return new(x - range, x + range + 1);
        }
    }
}

internal readonly record struct RangeRecord(int Start, int EndExclusive)
{
    internal int Length => EndExclusive - Start;

    internal bool TryJoin(RangeRecord other, out RangeRecord result)
    {
        result = new(Math.Min(Start, other.Start), Math.Max(EndExclusive, other.EndExclusive));
        return EndExclusive >= other.Start || Start <= other.EndExclusive;
    }

    internal bool Contains(int position) => Start <= position && position < EndExclusive;
}
