using System.Collections.Generic;
using System.Diagnostics;
using AdventOfCode2022.PartOne;
using AdventOfCode2022.PartTwo;
using Arborescence;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static IEnumerable<TNode> Traverse<TGraph, TNode>(TGraph graph, TNode source)
        where TGraph : IIncidenceGraph<TNode, TNode, IEnumerator<TNode>>
        where TNode : INode
    {
        // TODO: Implement with GenericSearch<>.
        HashSet<TNode> exploredSet = new() { source };
        yield return source;
        PriorityQueue<TNode, TNode> frontier = new(NodePriorityComparer<TGraph, TNode>.Instance);
        frontier.Enqueue(source, source);
        while (frontier.TryDequeue(out TNode? current, out _))
        {
            Debug.Assert(exploredSet.Contains(current));
            IEnumerator<TNode> neighbourEnumerator = graph.EnumerateOutEdges(current);
            while (neighbourEnumerator.MoveNext())
            {
                TNode neighbor = neighbourEnumerator.Current;
                if (!exploredSet.Contains(neighbor))
                {
                    exploredSet.Add(neighbor);
                    yield return neighbor;
                    frontier.Enqueue(neighbor, neighbor);
                }
            }
        }
    }
}

internal sealed class NodePriorityComparer<TGraph, TNode> : IComparer<TNode>
    where TGraph : IIncidenceGraph<TNode, TNode, IEnumerator<TNode>>
    where TNode : INode
{
    internal static NodePriorityComparer<TGraph, TNode> Instance { get; } = new();

    public int Compare(TNode? x, TNode? y)
    {
        TNode left = x!;
        if (left.DepthLeft == 0)
            return -1;
        TNode right = y!;
        if (right.DepthLeft == 0)
            return 1;
        int guaranteedComparison =
            GuaranteedTotalPressureReleased(right).CompareTo(GuaranteedTotalPressureReleased(left));
        if (guaranteedComparison != 0)
            return guaranteedComparison;
        int openBitsetComparison = ulong.PopCount(left.OpenBitset).CompareTo(ulong.PopCount(right.OpenBitset));
        return openBitsetComparison;
    }

    private static int GuaranteedTotalPressureReleased(TNode node) =>
        node.TotalPressureReleased + node.TotalFlowRate * node.DepthLeft;
}
