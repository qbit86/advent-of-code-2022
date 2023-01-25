using System.Collections.Generic;
using System.Linq;
using Arborescence.Traversal.Adjacency;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static int MaxGeodeCount(Blueprint blueprint, int elapsedMinutesBound)
    {
        Graph graph = new(blueprint, elapsedMinutesBound);
        Node source = new(0, 1, 0, 0, 0, default);
        IEnumerable<Node> nodes = EnumerableDfs<Node>.EnumerateVertices(graph, source);
        IEnumerable<int> geodes = from node in nodes
            where node.ElapsedMinutes == graph.ElapsedMinutesBound
            select node.Resources.Geode;
        return geodes.Max();
    }
}
