using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022.PartOne;

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
        ValveRecord[] valveRecordById = lines.Select(ValveRecord.Parse).ToArray();
        var graph = Graph.Create(valveRecordById);
        Node startNode = CreateStartNode(valveRecordById);
        IEnumerable<Node> nodes = Puzzles.Traverse(graph, startNode);
        IEnumerable<Node> filteredNodes = nodes.Where(it => it.DepthLeft == 0);
        long result = filteredNodes.Max(it => it.TotalPressureReleased);
        return result;
    }

    private static Node CreateStartNode(ValveRecord[] valveRecordById)
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

        return new(startValve.Id, openBitset, 0, 0, 30);
    }
}
