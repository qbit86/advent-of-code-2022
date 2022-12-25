using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

    internal static Point Add(this Point position, Size direction) => Point.Add(position, direction);

    internal static int Mod(this int dividend, int divisor) => (dividend % divisor + divisor) % divisor;
}
