using System;

namespace AdventOfCode2022;

internal static class Helpers
{
    internal const int PriorityUpperBound = 53;

    internal static int GetPriority(char item) => item switch
    {
        _ when char.IsAsciiLetterLower(item) => 1 + item - 'a',
        _ when char.IsAsciiLetterUpper(item) => 27 + item - 'A',
        _ => throw new ArgumentOutOfRangeException(nameof(item))
    };
}
