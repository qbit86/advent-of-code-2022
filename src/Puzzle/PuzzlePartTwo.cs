using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo : IPuzzle<long>
{
    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    private static long SolveCore(IReadOnlyList<string> lines)
    {
        JsonArray divider2 = new(new JsonArray(2));
        JsonArray divider6 = new(new JsonArray(6));
        var arrays = lines
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(Puzzles.Parse).ToList();
        arrays.Add(divider2);
        arrays.Add(divider6);
        arrays.Sort(ArrayComparer.Instance);
        int indexOf2 = arrays.IndexOf(divider2);
        int indexOf6 = arrays.IndexOf(divider6, indexOf2 + 1);
        long result = (indexOf2 + 1) * (indexOf6 + 1);
        return result;
    }
}
