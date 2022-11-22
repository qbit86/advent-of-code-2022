using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2022;

internal static class TextReaderExtensions
{
    internal static async Task<IReadOnlyList<string>> ReadAllLinesAsync(this TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);

        List<string> lines = new();
        while (true)
        {
            string? line = await input.ReadLineAsync().ConfigureAwait(false);
            if (line is null)
                break;

            lines.Add(line);
        }

        return lines;
    }
}
