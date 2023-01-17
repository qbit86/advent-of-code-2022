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
        IEnumerator<Node> firstTripEnumerator = EnumerableBfs<Node>.EnumerateVertices(graph, firstStart, exploredSet);
        while (firstTripEnumerator.MoveNext())
        {
            Node current = firstTripEnumerator.Current;
            if (current.Position == graph.Goal)
            {
                firstGoal = current;
                break;
            }
        }

        Debug.Assert(firstGoal.Position == graph.Goal);

        Node secondStart = default;
        exploredSet.Clear();
        IEnumerator<Node> secondTripEnumerator = EnumerableBfs<Node>.EnumerateVertices(graph, firstGoal, exploredSet);
        while (secondTripEnumerator.MoveNext())
        {
            Node current = secondTripEnumerator.Current;
            if (current.Position == Graph.Start)
            {
                secondStart = current;
                break;
            }
        }

        Debug.Assert(secondStart.Position == Graph.Start);

        Node secondGoal = default;
        exploredSet.Clear();
        IEnumerator<Node> thirdTripEnumerator = EnumerableBfs<Node>.EnumerateVertices(graph, secondStart, exploredSet);
        while (thirdTripEnumerator.MoveNext())
        {
            Node current = thirdTripEnumerator.Current;
            if (current.Position == graph.Goal)
            {
                secondGoal = current;
                break;
            }
        }

        Debug.Assert(secondGoal.Position == graph.Goal);

        return secondGoal.Time;
    }
}
