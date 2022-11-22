using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo : IPuzzle<long>
{
    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        IReadOnlyList<string> lines = await input.ReadAllLinesAsync().ConfigureAwait(false);
        return SolveCore(lines);
    }

    private long SolveCore(IReadOnlyList<string> lines) => throw new NotImplementedException();
}
