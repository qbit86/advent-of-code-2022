using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Arborescence;

namespace AdventOfCode2022.PartTwo;

internal sealed class GraphPartTwo : IIncidenceGraph<NodePartTwo, NodePartTwo, IEnumerator<NodePartTwo>>
{
    private readonly int _totalFlowRate;
    private readonly IReadOnlyList<ValveRecord> _valveRecordById;
    private readonly IReadOnlyDictionary<string, ValveRecord> _valveRecordByName;
    private int _guaranteedTotalPressureReleased;

    private GraphPartTwo(
        IReadOnlyList<ValveRecord> valveRecordById,
        IReadOnlyDictionary<string, ValveRecord> valveRecordByName,
        int totalFlowRate)
    {
        _valveRecordById = valveRecordById;
        _valveRecordByName = valveRecordByName;
        _totalFlowRate = totalFlowRate;
    }

    public bool TryGetHead(NodePartTwo edge, [UnscopedRef] out NodePartTwo head)
    {
        head = edge;
        return true;
    }

    public bool TryGetTail(NodePartTwo edge, [UnscopedRef] out NodePartTwo tail) => throw new NotSupportedException();

    public IEnumerator<NodePartTwo> EnumerateOutEdges(NodePartTwo vertex)
    {
        Debug.Assert(vertex.DepthLeft >= 0);
        if (vertex.DepthLeft == 0)
            yield break;

        if (AllOpen(vertex))
        {
            Debug.Assert(_totalFlowRate == vertex.TotalFlowRate);
            NodePartTwo neighborNodeCandidate = Wait(vertex);
            if (TotalPressureReleasedUpperBound(neighborNodeCandidate) > _guaranteedTotalPressureReleased)
            {
                _guaranteedTotalPressureReleased =
                    Math.Max(_guaranteedTotalPressureReleased, TotalPressureReleasedGuaranteed(neighborNodeCandidate));
                yield return neighborNodeCandidate;
            }

            yield break;
        }

        sbyte newDepthLeft = (sbyte)(vertex.DepthLeft - 1);

        ValveRecord humanValve = _valveRecordById[vertex.HumanValveId];
        IReadOnlyList<string> humanNeighborNames = humanValve.NeighborNames;
        int humanNeighborCount = humanNeighborNames.Count;

        ValveRecord elephantValve = _valveRecordById[vertex.ElephantValveId];
        IReadOnlyList<string> elephantNeighborNames = elephantValve.NeighborNames;
        int elephantNeighborCount = elephantNeighborNames.Count;

        if (!IsOpenForHuman(vertex) && !IsOpenForElephant(vertex) && humanValve.Id != elephantValve.Id)
        {
            ulong mask = (1UL << vertex.HumanValveId) | (1UL << vertex.ElephantValveId);
            NodePartTwo neighborNodeCandidate = new(
                Math.Min(vertex.HumanValveId, vertex.ElephantValveId),
                Math.Max(vertex.HumanValveId, vertex.ElephantValveId),
                DepthLeft: newDepthLeft,
                OpenBitset: vertex.OpenBitset | mask,
                TotalPressureReleased: vertex.TotalPressureReleased + vertex.TotalFlowRate,
                TotalFlowRate: (byte)(vertex.TotalFlowRate + humanValve.Rate + elephantValve.Rate));
            if (TotalPressureReleasedUpperBound(neighborNodeCandidate) >= _guaranteedTotalPressureReleased)
                yield return neighborNodeCandidate;
        }

        if (!IsOpenForHuman(vertex))
        {
            for (int elephantNeighborIndex = 0; elephantNeighborIndex < elephantNeighborCount; ++elephantNeighborIndex)
            {
                string elephantNeighborName = elephantNeighborNames[elephantNeighborIndex];
                ValveRecord neighborValve = _valveRecordByName[elephantNeighborName];
                ulong mask = 1UL << vertex.HumanValveId;
                NodePartTwo neighborNodeCandidate = new(
                    Math.Min(vertex.HumanValveId, neighborValve.Id),
                    Math.Max(vertex.HumanValveId, neighborValve.Id),
                    vertex.OpenBitset | mask,
                    vertex.TotalPressureReleased + vertex.TotalFlowRate,
                    (byte)(vertex.TotalFlowRate + humanValve.Rate),
                    newDepthLeft);
                if (TotalPressureReleasedUpperBound(neighborNodeCandidate) >= _guaranteedTotalPressureReleased)
                    yield return neighborNodeCandidate;
            }
        }

        if (!IsOpenForElephant(vertex))
        {
            for (int humanNeighborIndex = 0; humanNeighborIndex < humanNeighborCount; ++humanNeighborIndex)
            {
                string humanNeighborName = humanNeighborNames[humanNeighborIndex];
                ValveRecord neighborValve = _valveRecordByName[humanNeighborName];
                ulong mask = 1UL << vertex.ElephantValveId;
                NodePartTwo neighborNodeCandidate = new(
                    Math.Min(neighborValve.Id, vertex.ElephantValveId),
                    Math.Max(neighborValve.Id, vertex.ElephantValveId),
                    vertex.OpenBitset | mask,
                    vertex.TotalPressureReleased + vertex.TotalFlowRate,
                    (byte)(vertex.TotalFlowRate + elephantValve.Rate),
                    newDepthLeft);
                if (TotalPressureReleasedUpperBound(neighborNodeCandidate) >= _guaranteedTotalPressureReleased)
                    yield return neighborNodeCandidate;
            }
        }

        for (int humanNeighborIndex = 0; humanNeighborIndex < humanNeighborCount; ++humanNeighborIndex)
        {
            string humanNeighborName = humanNeighborNames[humanNeighborIndex];
            ValveRecord humanNeighborValve = _valveRecordByName[humanNeighborName];
            for (int elephantNeighborIndex = 0; elephantNeighborIndex < elephantNeighborCount; ++elephantNeighborIndex)
            {
                string elephantNeighborName = elephantNeighborNames[elephantNeighborIndex];
                ValveRecord elephantNeighborValve = _valveRecordByName[elephantNeighborName];

                if (humanValve.Id == elephantValve.Id && humanNeighborValve.Id > elephantNeighborValve.Id)
                    continue;

                NodePartTwo neighborNodeCandidate = vertex with
                {
                    HumanValveId = Math.Min(humanNeighborValve.Id, elephantNeighborValve.Id),
                    ElephantValveId = Math.Max(humanNeighborValve.Id, elephantNeighborValve.Id),
                    DepthLeft = newDepthLeft,
                    TotalPressureReleased = vertex.TotalPressureReleased + vertex.TotalFlowRate
                };
                if (TotalPressureReleasedUpperBound(neighborNodeCandidate) > _guaranteedTotalPressureReleased)
                {
                    _guaranteedTotalPressureReleased = Math.Max(
                        _guaranteedTotalPressureReleased, TotalPressureReleasedGuaranteed(neighborNodeCandidate));
                    yield return neighborNodeCandidate;
                }
            }
        }
    }

    internal static GraphPartTwo Create(IReadOnlyList<ValveRecord> valveRecordById)
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

    private bool AllOpen(in NodePartTwo node) => (int)ulong.PopCount(node.OpenBitset) == _valveRecordById.Count;

    private static bool IsOpenForHuman(in NodePartTwo node)
    {
        ulong mask = 1UL << node.HumanValveId;
        return (node.OpenBitset & mask) != 0UL;
    }

    private static bool IsOpenForElephant(in NodePartTwo node)
    {
        ulong mask = 1UL << node.ElephantValveId;
        return (node.OpenBitset & mask) != 0UL;
    }

    private int TotalPressureReleasedUpperBound(NodePartTwo node) =>
        node.TotalPressureReleased + node.DepthLeft * _totalFlowRate;

    private static int TotalPressureReleasedGuaranteed(NodePartTwo node) =>
        node.TotalPressureReleased + node.DepthLeft * node.TotalFlowRate;

    private static NodePartTwo Wait(in NodePartTwo node) => node with
    {
        HumanValveId = Math.Min(node.HumanValveId, node.ElephantValveId),
        ElephantValveId = Math.Max(node.HumanValveId, node.ElephantValveId),
        DepthLeft = 0,
        TotalPressureReleased = node.TotalPressureReleased + node.DepthLeft * node.TotalFlowRate
    };
}
