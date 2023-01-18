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
        var cubes = lines.Select(Puzzles.Parse).ToHashSet();
        return Puzzles.ComputeSurfaceArea(cubes);
    }
}
