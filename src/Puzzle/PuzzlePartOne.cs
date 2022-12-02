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

    private long SolveCore(IReadOnlyList<string> lines) => lines.Select(ComputeScore).Sum();

    private int ComputeScore(string line) => ComputeScore(line[0], line[^1]);

    private int ComputeScore(char left, char right)
    {
        int leftNormalized = left - 'A';
        int rightNormalized = right - 'X';

        if (unchecked((uint)leftNormalized >= 3))
            throw new ArgumentOutOfRangeException(nameof(left));
        if (unchecked((uint)rightNormalized >= 3))
            throw new ArgumentOutOfRangeException(nameof(right));

        return Helpers.ComputeShapeScore(rightNormalized) + ComputeOutcomeScore(leftNormalized, rightNormalized);
    }

    private int ComputeOutcomeScore(int left, int right) => (Comparer.Instance.Compare(right, left) + 1) * 3;
}
