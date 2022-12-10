using System;

namespace AdventOfCode2022;

internal readonly record struct Command(CommandKind Kind, int Argument)
{
    internal static Command Parse(string line)
    {
        if (line is "noop")
            return new(CommandKind.Noop, int.MinValue);
        if (line.StartsWith("addx "))
            return new(CommandKind.Addx, int.Parse(line.AsSpan(5)));
        throw new ArgumentException(line, nameof(line));
    }
}
