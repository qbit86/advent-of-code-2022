using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022.PartTwo;

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
        ValveRecord[] valveRecordById = lines.Select(ValveRecord.Parse).ToArray();
        var graph = GraphPartTwo.Create(valveRecordById);
        NodePartTwo startNode = CreateStartNode(valveRecordById);
        IEnumerable<NodePartTwo> nodes = Puzzles.Traverse(graph, startNode);
        IEnumerable<NodePartTwo> filteredNodes = nodes.Where(it => it.DepthLeft == 0);
        long result = filteredNodes.Max(it => it.TotalPressureReleased);
        return result;
    }

    private static NodePartTwo CreateStartNode(ValveRecord[] valveRecordById)
    {
        int startValveIndex = Array.FindIndex(valveRecordById, it => it.Name == "AA");
        ValveRecord startValve = valveRecordById[startValveIndex];
        ulong openBitset = 0UL;
        for (int i = 0; i < valveRecordById.Length; ++i)
        {
            if (valveRecordById[i].Rate != 0)
                continue;
            ulong mask = 1UL << i;
            openBitset |= mask;
        }

        return new(startValve.Id, startValve.Id, openBitset, 0, 0, 26);
    }
}
