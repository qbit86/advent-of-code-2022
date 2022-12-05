using System;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static void PopulateSpan(Span<char> span, ProblemData data)
    {
        for (int i = 0; i < data.StackCount; ++i)
            span[i] = data.Stacks[i].Peek();
    }
}
