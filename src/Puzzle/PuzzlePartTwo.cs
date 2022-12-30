using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo
{
    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve(IReadOnlyList<string> lines) => throw new NotImplementedException();
}
