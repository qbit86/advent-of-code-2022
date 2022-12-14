using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        HashSet<Point> sandUnits = new();
        var simulation = SimulationPartTwo.Create(lines, sandUnits);
        Point initialPosition = new(500, 0);
        for (long result = 0L;; ++result)
        {
            if (simulation.TrySimulateSandUnit(initialPosition, out Point finalPosition))
                sandUnits.Add(finalPosition);
            else
                return result;
        }
    }
}
