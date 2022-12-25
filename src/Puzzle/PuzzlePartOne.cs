using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartOne : IPuzzle<long>
{
    private readonly int _rockCount;

    public PuzzlePartOne(int rockCount) => _rockCount = rockCount;

    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        string? line = await input.ReadLineAsync().ConfigureAwait(false);
        return SolveCore(line ?? throw new ArgumentException(null, nameof(input)));
    }

    private long SolveCore(string jets)
    {
        List<byte> stoppedBlocks = new(_rockCount);
        SimulatorSlim simulator = new(jets, stoppedBlocks);
        return simulator.Simulate(0, 0, _rockCount, out _);
    }
}
