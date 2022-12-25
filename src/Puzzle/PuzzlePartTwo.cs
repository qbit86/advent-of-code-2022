using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo : IPuzzle<long>
{
    private readonly long _rockCount;

    public PuzzlePartTwo(long rockCount) => _rockCount = rockCount;

    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        string? line = await input.ReadLineAsync().ConfigureAwait(false);
        return SolveCore(line ?? throw new ArgumentException(null, nameof(input)));
    }

    private long SolveCore(string jets)
    {
        Dictionary<StateKey, StateData> dataByKey = new();
        List<byte> stoppedBlocks = new();
        SimulatorSlim simulator = new(jets, stoppedBlocks);
        const int byteCount = sizeof(ulong);
        int warmupDistance = Math.Max(byteCount, jets.Length);
        for (int rockIndex = 0, jetIndex = 0; rockIndex < _rockCount; ++rockIndex)
        {
            int height = simulator.Simulate(rockIndex, jetIndex, out jetIndex);
            if (stoppedBlocks.Count < warmupDistance)
                continue;

            ReadOnlySpan<byte> stoppedBlocksSpan = CollectionsMarshal.AsSpan(stoppedBlocks);
            ReadOnlySpan<byte> lastLines = stoppedBlocksSpan[^byteCount..];
            ulong lastLinesData = BinaryPrimitives.ReadUInt64BigEndian(lastLines);
            int rockIndexWrapped = rockIndex % SimulatorSlim.ModelCount;
            int jetIndexWrapped = jetIndex % jets.Length;
            StateKey stateKey = new(lastLinesData, rockIndexWrapped, jetIndexWrapped);
            if (dataByKey.TryGetValue(stateKey, out StateData stateData))
            {
                int rockIndexDiff = rockIndex - stateData.RockIndex;
                (long periodCount, long remainder) = Math.DivRem(_rockCount - stateData.RockIndex - 1L, rockIndexDiff);
                int heightDiff = height - stateData.Height;
                long updatedHeight = remainder > 0
                    ? simulator.Simulate(rockIndex + 1, jetIndex, (int)remainder, out jetIndex)
                    : height;
                long result = updatedHeight + (periodCount - 1L) * heightDiff;
                return result;
            }

            dataByKey.Add(stateKey, new(rockIndex, simulator.Height));
        }

        return simulator.Height;
    }
}

internal readonly record struct StateKey(ulong LastLinesData, int RockIndexWrapped, int JetIndexWrapped);

internal readonly record struct StateData(int RockIndex, int Height);
