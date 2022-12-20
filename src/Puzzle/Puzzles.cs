using System.Numerics;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static T Mod<T>(T dividend, T divisor)
        where T : IAdditionOperators<T, T, T>, IModulusOperators<T, T, T> =>
        (dividend % divisor + divisor) % divisor;
}
