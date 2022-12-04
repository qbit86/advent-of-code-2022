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

    private static long SolveCore(IReadOnlyList<string> lines) =>
        lines.Select(Helpers.ParsePair).Count(pair => FullyOverlap(pair.Item1, pair.Item2));

    private static bool FullyOverlap(Range left, Range right) => Contains(left, right) || Contains(right, left);

    private static bool Contains(Range left, Range right) =>
        left.Start.Value <= right.Start.Value && right.End.Value <= left.End.Value;
}
