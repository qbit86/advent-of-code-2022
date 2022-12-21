using System.Collections.Generic;
using Arborescence.Traversal;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static int MaxGeodeCount(Blueprint blueprint, int elapsedMinutesBound)
    {
        EnumerableDfs<Graph, Node, Node, IEnumerator<Node>> dfs = new();
        Graph graph = new(blueprint, elapsedMinutesBound);
        Node source = new(0, 1, 0, 0, 0, default);
        HashSet<Node> exploredSet = new();
        IEnumerator<Node> nodesEnumerator = dfs.EnumerateVertices(graph, source, exploredSet);
        int result = default;
        while (nodesEnumerator.MoveNext())
        {
            Node current = nodesEnumerator.Current;
            if (current.ElapsedMinutes != graph.ElapsedMinutesBound)
                continue;
            int geodeCount = current.Resources.Geode;
            if (geodeCount > result)
                result = geodeCount;
        }

        return result;
    }
}
