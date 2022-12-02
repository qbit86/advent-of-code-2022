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

    private long SolveCore(IReadOnlyList<string> lines) => lines.Select(ComputeScore).Sum();

    private int ComputeScore(string line) => ComputeScore(line[0], line[^1]);

    private int ComputeScore(char left, char right)
    {
        int leftNormalized = left - 'A';
        int outcomeNormalized = right - 'X' - 1;

        if (unchecked((uint)leftNormalized >= 3))
            throw new ArgumentOutOfRangeException(nameof(left));
        if (outcomeNormalized is < -1 or > 1)
            throw new ArgumentOutOfRangeException(nameof(right));

        int rightNormalized = (leftNormalized + outcomeNormalized + 3) % 3;
        return Helpers.ComputeShapeScore(rightNormalized) + ComputeOutcomeScore(outcomeNormalized);
    }

    private int ComputeOutcomeScore(int outcome) => (outcome + 1) * 3;
}
