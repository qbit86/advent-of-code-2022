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
        var commands = lines.Select(Command.Parse).ToList();
        Cpu cpu = new(commands);
        var xs = cpu.GetRegisterValues().ToList();
        var filtered = xs
            .Select((x, index) => (x, index))
            .Where(it => it.index % 40 == 20)
            .ToList();
        var signalStrengths = filtered
            .Select(it => (long)it.x * it.index)
            .ToList();

        long result = signalStrengths.Sum();
        return result;
    }
}
