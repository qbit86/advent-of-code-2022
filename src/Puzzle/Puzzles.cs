using System.Collections.Generic;
using AdventOfCode2022.PartOne;
using AdventOfCode2022.PartTwo;
using Arborescence;
using Arborescence.Traversal.Adjacency;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static IEnumerable<TNode> Traverse<TGraph, TNode>(TGraph graph, TNode source)
        where TGraph : IAdjacency<TNode, IEnumerator<TNode>>
        where TNode : INode
    {
        Frontier<TNode> frontier = new(NodePriorityComparer<TNode>.Instance);
        IEnumerator<TNode> enumerator = EnumerableGenericSearch<TNode>.EnumerateVertices(graph, source, frontier);
        while (enumerator.MoveNext())
            yield return enumerator.Current;
    }
}
