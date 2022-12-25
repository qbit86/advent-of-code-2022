using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022;

internal static class Extensions
{
    internal static async Task ReadAllLinesAsync<TLines>(this TextReader input, TLines lines)
        where TLines : ICollection<string>
    {
        ArgumentNullException.ThrowIfNull(input);

        while (true)
        {
            string? line = await input.ReadLineAsync().ConfigureAwait(false);
            if (line is null)
                break;

            lines.Add(line);
        }
    }

    internal static char CharacterAtOrZero(this string line, Index index)
    {
        char c = line.ElementAtOrDefault(index);
        return c == default ? '0' : c;
    }
}
