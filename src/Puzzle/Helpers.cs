using System;

namespace AdventOfCode2022;

internal static class Helpers
{
    private const StringSplitOptions SplitOptions =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    internal static (Range, Range) ParsePair(string line)
    {
        string[] parts = line.Split(',', SplitOptions);
        if (parts.Length < 2)
            throw new ArgumentException(nameof(parts) + ".Length < 2", nameof(line));

        return (ParseRange(parts[0]), ParseRange(parts[^1]));
    }

    private static Range ParseRange(string range)
    {
        string[] parts = range.Split('-', SplitOptions);
        if (parts.Length < 2)
            throw new ArgumentException(nameof(parts) + ".Length < 2", nameof(range));

        return new(int.Parse(parts[0]), int.Parse(parts[^1]));
    }
}
