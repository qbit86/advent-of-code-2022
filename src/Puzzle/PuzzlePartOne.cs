using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartOne : IPuzzle<long>
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
        var chunks = lines
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(Puzzles.Parse).Chunk(2).ToList();
        var comparisons = chunks
            .Select((it, index) =>
                (index: index + 1, comparison: ArrayComparer.Instance.Compare(it[0], it[1]) <= 0))
            .Where(it => it.comparison).ToList();
        long result = comparisons.Select(it => it.index).Sum();
        return result;
    }
}
