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

    private static long SolveCore(IReadOnlyList<string> lines) =>
        lines.Select(Helpers.ParsePair).Count(pair => PartiallyOverlap(pair.Item1, pair.Item2));

    private static bool PartiallyOverlap(Range left, Range right) =>
        Contains(left, right.Start) || Contains(right, left.Start);

    private static bool Contains(Range range, Index point) =>
        range.Start.Value <= point.Value && point.Value <= range.End.Value;
}
