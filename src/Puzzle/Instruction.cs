using System;

namespace AdventOfCode2022;

internal readonly record struct Instruction(int Count, int From, int To) : IParsable<Instruction>
{
    internal static Instruction Parse(string s) => Parse(s, null);

    public static Instruction Parse(string s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out Instruction result))
            throw new ArgumentException("TryParse: false", nameof(s));

        return result;
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out Instruction result)
    {
        ArgumentException.ThrowIfNullOrEmpty(s);

        string[] parts = s.Split(' ', 6, StringSplitOptions.TrimEntries);
        if (!int.TryParse(parts[1], out int count))
            return Failure(out result);
        if (!int.TryParse(parts[3], out int from))
            return Failure(out result);
        if (!int.TryParse(parts[5], out int to))
            return Failure(out result);

        result = new(count, from - 1, to - 1);
        return true;

        static bool Failure(out Instruction r)
        {
            r = default;
            return false;
        }
    }
}
