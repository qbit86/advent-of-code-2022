using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Arborescence.Traversal.Adjacency;

namespace AdventOfCode2022;

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
        Graph graph = Puzzles.Parse(lines);

        Node source = new(Graph.Start, 0);
        Point targetPosition = graph.Goal;
        IEnumerator<Node> nodeEnumerator = EnumerableBfs<Node>.EnumerateVertices(graph, source);
        while (nodeEnumerator.MoveNext())
        {
            Node current = nodeEnumerator.Current;
            if (current.Position == targetPosition)
                return current.Time;
        }

        throw new UnreachableException();
    }
}
