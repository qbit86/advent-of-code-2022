using System;
using System.Collections.Generic;
using System.Drawing;
using AdventOfCode2022.PartOne;
using AdventOfCode2022.PartTwo;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static Point FindInitialPosition(IReadOnlyList<string> tileKindByPosition)
    {
        string row = tileKindByPosition[0];
        int columnIndex = row.IndexOf('.');
        if (columnIndex < 0)
            throw new InvalidOperationException(row);
        return new(columnIndex, 0);
    }
}
