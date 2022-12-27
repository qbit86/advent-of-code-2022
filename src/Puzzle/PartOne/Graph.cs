using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Arborescence;

namespace AdventOfCode2022.PartOne;

internal sealed class Graph : IIncidenceGraph<Node, Node, IEnumerator<Node>>
{
    private readonly int _totalFlowRate;
    private readonly IReadOnlyList<ValveRecord> _valveRecordById;
    private readonly IReadOnlyDictionary<string, ValveRecord> _valveRecordByName;
    private int _guaranteedTotalPressureReleased;

    private Graph(
        IReadOnlyList<ValveRecord> valveRecordById,
        IReadOnlyDictionary<string, ValveRecord> valveRecordByName,
        int totalFlowRate)
    {
        _valveRecordById = valveRecordById;
        _valveRecordByName = valveRecordByName;
        _totalFlowRate = totalFlowRate;
    }

    public bool TryGetHead(Node edge, [UnscopedRef] out Node head)
    {
        head = edge;
        return true;
    }

    public bool TryGetTail(Node edge, [UnscopedRef] out Node tail) => throw new NotSupportedException();

    public IEnumerator<Node> EnumerateOutEdges(Node vertex)
    {
        Debug.Assert(vertex.DepthLeft >= 0);
        if (vertex.DepthLeft == 0)
            yield break;

        if (AllOpen(vertex))
        {
            Debug.Assert(_totalFlowRate == vertex.TotalFlowRate);
            Node neighborNodeCandidate = Wait(vertex);
            if (TotalPressureReleasedUpperBound(neighborNodeCandidate) > _guaranteedTotalPressureReleased)
            {
                _guaranteedTotalPressureReleased =
                    Math.Max(_guaranteedTotalPressureReleased, TotalPressureReleasedGuaranteed(neighborNodeCandidate));
                yield return neighborNodeCandidate;
            }

            yield break;
        }

        sbyte newDepthLeft = (sbyte)(vertex.DepthLeft - 1);
        if (!IsOpen(vertex))
        {
            Node neighborNode = Open(vertex, newDepthLeft);
            yield return neighborNode;
        }

        ValveRecord valve = _valveRecordById[vertex.ValveId];
        IReadOnlyList<string> neighborNames = valve.NeighborNames;
        int neighborCount = neighborNames.Count;
        for (int i = 0; i < neighborCount; ++i)
        {
            string neighborName = neighborNames[i];
            ValveRecord neighborValve = _valveRecordByName[neighborName];
            Node neighborNodeCandidate = Move(vertex, newDepthLeft, neighborValve);
            if (TotalPressureReleasedUpperBound(neighborNodeCandidate) > _guaranteedTotalPressureReleased)
            {
                _guaranteedTotalPressureReleased =
                    Math.Max(_guaranteedTotalPressureReleased, TotalPressureReleasedGuaranteed(neighborNodeCandidate));
                yield return neighborNodeCandidate;
            }
        }
    }

    internal static Graph Create(IReadOnlyList<ValveRecord> valveRecordById)
    {
        var valveRecordByName = valveRecordById.ToDictionary(it => it.Name, it => it);
        // Patch valve records.
        foreach (ValveRecord valveRecord in valveRecordById)
        {
            if (valveRecord.NeighborNames is string[] array)
            {
                Array.Sort(
                    array, (left, right) => valveRecordByName[right].Rate.CompareTo(valveRecordByName[left].Rate));
            }
        }

        int totalFlowRate = valveRecordById.Select(it => it.Rate).Sum();
        return new(valveRecordById, valveRecordByName, totalFlowRate);
    }

    private bool AllOpen(in Node node) => (int)ulong.PopCount(node.OpenBitset) == _valveRecordById.Count;

    private static bool IsOpen(in Node node)
    {
        ulong mask = 1UL << node.ValveId;
        return (node.OpenBitset & mask) != 0UL;
    }

    private int TotalPressureReleasedUpperBound(Node node) =>
        node.TotalPressureReleased + node.DepthLeft * _totalFlowRate;

    private static int TotalPressureReleasedGuaranteed(Node node) =>
        node.TotalPressureReleased + node.DepthLeft * node.TotalFlowRate;

    private static Node Wait(in Node node) => node with
    {
        DepthLeft = 0,
        TotalPressureReleased = node.TotalPressureReleased + node.DepthLeft * node.TotalFlowRate
    };

    private Node Open(in Node node, sbyte newDepthLeft)
    {
        ulong mask = 1UL << node.ValveId;
        ValveRecord valve = _valveRecordById[node.ValveId];
        return node with
        {
            DepthLeft = newDepthLeft,
            OpenBitset = node.OpenBitset | mask,
            TotalPressureReleased = node.TotalPressureReleased + node.TotalFlowRate,
            TotalFlowRate = (byte)(node.TotalFlowRate + valve.Rate)
        };
    }

    private static Node Move(in Node node, sbyte newDepthLeft, ValveRecord neighborValve) => node with
    {
        ValveId = neighborValve.Id,
        DepthLeft = newDepthLeft,
        TotalPressureReleased = node.TotalPressureReleased + node.TotalFlowRate
    };
}
