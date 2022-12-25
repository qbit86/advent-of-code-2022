using System;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static Graph Parse(IReadOnlyList<string> lines)
    {
        int rowCount = lines.Count - 2;
        List<Size> directionByBlizzard = new();
        List<Point> positionByBlizzard = new();
        int columnCount = int.MaxValue;
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            string line = lines[rowIndex + 1];
            string row = line.Substring(1, line.Length - 2);
            columnCount = Math.Min(columnCount, row.Length);
            for (int columnIndex = 0; columnIndex < row.Length; ++columnIndex)
            {
                if (TryGetBlizzardDirection(row[columnIndex], out Size direction))
                {
                    positionByBlizzard.Add(new(columnIndex, rowIndex));
                    directionByBlizzard.Add(direction);
                }
            }
        }

        var graph = Graph.Create(new(columnCount, rowCount), directionByBlizzard, positionByBlizzard);
        return graph;
    }

    private static bool TryGetBlizzardDirection(char c, out Size direction)
    {
        direction = GetBlizzardDirectionOrDefault(c);
        return direction != Size.Empty;
    }

    private static Size GetBlizzardDirectionOrDefault(char c) => c switch
    {
        '>' => D.UnitX,
        'v' => D.UnitY,
        '<' => D.MinusUnitX,
        '^' => D.MinusUnitY,
        _ => Size.Empty
    };
}
