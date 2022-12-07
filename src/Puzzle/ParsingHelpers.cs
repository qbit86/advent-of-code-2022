using System;
using System.Diagnostics.CodeAnalysis;
using static AdventOfCode2022.TryHelpers;

namespace AdventOfCode2022;

internal static class ParsingHelpers
{
    private const string CdCommandPrefix = "$ cd ";
    private const string DirPrefix = "dir ";

    private const StringSplitOptions SplitOptions =
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

    private static readonly string[] s_cdCommandSeparators = { CdCommandPrefix };
    private static readonly string[] s_dirSeparators = { DirPrefix };

    internal static bool TryMatchCdCommand(string line, [NotNullWhen(true)] out string? argument)
    {
        if (!line.StartsWith(CdCommandPrefix))
            return None(out argument);
        string[] parts = line.Split(s_cdCommandSeparators, SplitOptions);
        return parts.Length == 0 ? None(out argument) : Some(parts[^1], out argument);
    }

    internal static bool TryMatchDir(string line, [NotNullWhen(true)] out string? segment)
    {
        if (!line.StartsWith(DirPrefix))
            return None(out segment);
        string[] parts = line.Split(s_dirSeparators, SplitOptions);
        return parts.Length == 0 ? None(out segment) : Some(parts[^1], out segment);
    }

    internal static bool TryMatchFile(string line, out int size, [NotNullWhen(true)] out string? fileName)
    {
        string[] parts = line.Split(' ', SplitOptions);
        if (parts.Length == 2)
            return int.TryParse(parts[0], out size) ? Some(parts[^1], out fileName) : None(out fileName);

        size = default;
        return None(out fileName);
    }
}
