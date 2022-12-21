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
        Blueprint[] blueprints = lines.Select(Blueprint.Parse).ToArray();
        long result = blueprints.Select(ComputeQualityLevel).Sum();
        return result;
    }

    private static long ComputeQualityLevel(Blueprint blueprint) => blueprint.Id * Puzzles.MaxGeodeCount(blueprint, 24);
}
