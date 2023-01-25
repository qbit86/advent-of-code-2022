using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Arborescence.Traversal.Adjacency;

namespace AdventOfCode2022;

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
        Graph graph = Puzzles.Parse(lines);

        Node firstStart = new(Graph.Start, 0);
        Node firstGoal = default;
        HashSet<Node> exploredSet = new();
        IEnumerable<Node> firstTripNodes = EnumerableBfs<Node>.EnumerateVertices(graph, firstStart, exploredSet);
        foreach (Node node in firstTripNodes)
        {
            if (node.Position == graph.Goal)
            {
                firstGoal = node;
                break;
            }
        }

        Debug.Assert(firstGoal.Position == graph.Goal);

        Node secondStart = default;
        exploredSet.Clear();
        IEnumerable<Node> secondTripNodes = EnumerableBfs<Node>.EnumerateVertices(graph, firstGoal, exploredSet);
        foreach (Node node in secondTripNodes)
        {
            if (node.Position == Graph.Start)
            {
                secondStart = node;
                break;
            }
        }

        Debug.Assert(secondStart.Position == Graph.Start);

        Node secondGoal = default;
        exploredSet.Clear();
        IEnumerable<Node> thirdTripNodes = EnumerableBfs<Node>.EnumerateVertices(graph, secondStart, exploredSet);
        foreach (Node node in thirdTripNodes)
        {
            if (node.Position == graph.Goal)
            {
                secondGoal = node;
                break;
            }
        }

        Debug.Assert(secondGoal.Position == graph.Goal);

        return secondGoal.Time;
    }
}
