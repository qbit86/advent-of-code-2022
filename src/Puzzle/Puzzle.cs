using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class Puzzle : IPuzzle<long>
{
    private int KnotCount { get; }

    public Puzzle(int knotCount) => KnotCount = knotCount;

    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    private long SolveCore(IReadOnlyList<string> lines)
    {
        Command[] commands = lines.Select(Command.Parse).ToArray();
        Simulation simulation = new(commands.Length, KnotCount);
        foreach (Command command in commands)
            simulation.Update(command);

        return simulation.TailPositions.Count;
    }
}
