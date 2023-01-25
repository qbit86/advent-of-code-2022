using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        IEnumerable<Node> nodes = EnumerableBfs<Node>.EnumerateVertices(graph, source);
        return nodes.Where(it => it.Position == targetPosition).Select(it => it.Time).First();
    }
}
