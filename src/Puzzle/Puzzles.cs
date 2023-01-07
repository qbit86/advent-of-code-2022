using System.Collections.Generic;
using AdventOfCode2022.PartOne;
using AdventOfCode2022.PartTwo;
using Arborescence;
using Arborescence.Traversal;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static IEnumerable<TNode> Traverse<TGraph, TNode>(TGraph graph, TNode source)
        where TGraph : IHeadIncidence<TNode, TNode>, IOutEdgesIncidence<TNode, IEnumerator<TNode>>
        where TNode : INode
    {
        GenericSearch<TGraph, TNode, TNode, IEnumerator<TNode>> search = new();
        HashSet<TNode> exploredSet = new();
        Frontier<TNode> frontier = new(NodePriorityComparer<TNode>.Instance);
        IEnumerator<TNode> enumerator = search.EnumerateVertices(graph, source, frontier, exploredSet);
        while (enumerator.MoveNext())
            yield return enumerator.Current;
    }
}
