namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static int ParseHeight(char c) => c switch { 'S' => 0, 'E' => 'z' - 'a', _ => c - 'a' };
}
