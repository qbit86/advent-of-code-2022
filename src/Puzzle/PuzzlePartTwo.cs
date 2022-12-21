using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        Blueprint[] blueprints = lines.Take(3).Select(Blueprint.Parse).ToArray();
        long result = blueprints.Select(MaxGeodeCount).Aggregate(1L, (left, right) => left * right);
        return result;

        static int MaxGeodeCount(Blueprint blueprint) => Puzzles.MaxGeodeCount(blueprint, 32);
    }
}
