using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static HashSet<Point> CreateElfPositions(IReadOnlyList<string> lines)
    {
        HashSet<Point> elfPositions = new(lines.Count);
        for (int rowIndex = 0; rowIndex < lines.Count; ++rowIndex)
        {
            string row = lines[rowIndex];
            for (int columnIndex = 0; columnIndex < row.Length; ++columnIndex)
            {
                if (row[columnIndex] == '#')
                    elfPositions.Add(new(columnIndex, rowIndex));
            }
        }

        return elfPositions;
    }

    private static void PopulateMooreNeighborhood(Point position, Span<Point> buffer)
    {
        buffer[0] = position with { X = position.X + 1 };
        buffer[1] = position with { X = position.X + 1, Y = position.Y + 1 };
        buffer[2] = position with { Y = position.Y + 1 };
        buffer[3] = position with { X = position.X - 1, Y = position.Y + 1 };
        buffer[4] = position with { X = position.X - 1 };
        buffer[5] = position with { X = position.X - 1, Y = position.Y - 1 };
        buffer[6] = position with { Y = position.Y - 1 };
        buffer[7] = position with { X = position.X + 1, Y = position.Y - 1 };
    }

    internal static IEnumerable<int> Simulate(HashSet<Point> elfPositions)
    {
        Queue<Size[]> directions = new(4);
        directions.Enqueue(new Size[] { new(-1, -1), new(0, -1), new(1, -1) });
        directions.Enqueue(new Size[] { new(-1, 1), new(0, 1), new(1, 1) });
        directions.Enqueue(new Size[] { new(-1, -1), new(-1, 0), new(-1, 1) });
        directions.Enqueue(new Size[] { new(1, -1), new(1, 0), new(1, 1) });

        Point[] mooreNeighborhood = GC.AllocateUninitializedArray<Point>(8);
        Dictionary<Point, Point> proposedBySource = new(elfPositions.Count);
        for (int round = 1;; ++round)
        {
            proposedBySource.Clear();

            foreach (Point elfPosition in elfPositions)
            {
                PopulateMooreNeighborhood(elfPosition, mooreNeighborhood);
                if (!mooreNeighborhood.Any(elfPositions.Contains))
                {
                    proposedBySource.Add(elfPosition, elfPosition);
                    continue;
                }

                foreach (Size[] candidate in directions)
                {
                    Point[] frontier = candidate.Select(it => Point.Add(elfPosition, it)).ToArray();
                    if (!frontier.Any(elfPositions.Contains))
                    {
                        proposedBySource[elfPosition] = frontier[1];
                        break;
                    }
                }

                if (!proposedBySource.ContainsKey(elfPosition))
                    proposedBySource.Add(elfPosition, elfPosition);
            }

            ILookup<Point, Point> sourcesByProposed = proposedBySource.ToLookup(kv => kv.Value, kv => kv.Key);
            var pairs = sourcesByProposed
                .Where(grouping => grouping.Count() == 1)
                .Select(grouping => new SourceProposedPair(grouping.Single(), grouping.Key))
                .Where(it => it.Source != it.Proposed)
                .ToList();
            if (pairs.Count == 0)
            {
                yield return round;
                yield break;
            }

            foreach ((Point source, Point proposed) in pairs)
            {
                bool wasFoundAndRemoved = elfPositions.Remove(source);
                Debug.Assert(wasFoundAndRemoved);
                bool wasAdded = elfPositions.Add(proposed);
                Debug.Assert(wasAdded);
            }

            directions.Enqueue(directions.Dequeue());
            yield return round;
        }
    }
}

internal readonly record struct SourceProposedPair(Point Source, Point Proposed);
