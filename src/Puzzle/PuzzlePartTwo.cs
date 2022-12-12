using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Arborescence;
using Arborescence.Models;

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
        if (!TryFindDestination(lines, out int destination))
            throw new ArgumentException($"{nameof(TryFindDestination)}: false", nameof(lines));

        SimpleIncidenceGraph graph = CreateGraph(lines);

        int source = destination;
        int[] distanceByVertex = GC.AllocateUninitializedArray<int>(graph.VertexCount);
        distanceByVertex.AsSpan().Fill(int.MaxValue);
        distanceByVertex[source] = 0;
        int?[] predecessorByVertex = new int?[graph.VertexCount];
        predecessorByVertex[source] = source;
        Queue<int> frontier = new();
        frontier.Enqueue(source);
        int columnCount = lines[0].Length;
        while (frontier.TryDequeue(out int current))
        {
            (int rowIndex, int columnIndex) = Math.DivRem(current, columnCount);
            int currentHeight = Puzzles.ParseHeight(lines[rowIndex][columnIndex]);
            if (currentHeight == 0)
                return distanceByVertex[current];
            Debug.Assert(predecessorByVertex[current].HasValue);
            ArraySegment<Endpoints>.Enumerator edges = graph.EnumerateOutEdges(current);
            while (edges.MoveNext())
            {
                int neighbor = edges.Current.Head;
                if (!predecessorByVertex[neighbor].HasValue)
                {
                    predecessorByVertex[neighbor] = current;
                    distanceByVertex[neighbor] = distanceByVertex[current] + 1;
                    frontier.Enqueue(neighbor);
                }
            }
        }

        throw new ArgumentException(null, nameof(lines));
    }

    private static SimpleIncidenceGraph CreateGraph(IReadOnlyList<string> lines)
    {
        int rowCount = lines.Count;
        Debug.Assert(rowCount > 0);
        int columnCount = lines[0].Length;
        Debug.Assert(columnCount > 0);
        int vertexCount = rowCount * columnCount;
        SimpleIncidenceGraph.Builder builder = new(vertexCount, 4);
        for (int rowIndex = 0, tail = 0; rowIndex < rowCount; ++rowIndex)
        {
            string row = lines[rowIndex];
            if (row.Length != columnCount)
                throw new ArgumentException(row, nameof(lines));
            for (int columnIndex = 0; columnIndex < columnCount; ++columnIndex, ++tail)
            {
                int tailHeight = Puzzles.ParseHeight(row[columnIndex]);
                if (columnIndex < columnCount - 1)
                {
                    int headHeight = Puzzles.ParseHeight(lines[rowIndex][columnIndex + 1]);
                    if (tailHeight <= headHeight + 1)
                    {
                        int head = rowIndex * columnCount + columnIndex + 1;
                        builder.Add(tail, head);
                    }
                }

                if (rowIndex > 0)
                {
                    int headHeight = Puzzles.ParseHeight(lines[rowIndex - 1][columnIndex]);
                    if (tailHeight <= headHeight + 1)
                    {
                        int head = (rowIndex - 1) * columnCount + columnIndex;
                        builder.Add(tail, head);
                    }
                }

                if (columnIndex > 0)
                {
                    int headHeight = Puzzles.ParseHeight(lines[rowIndex][columnIndex - 1]);
                    if (tailHeight <= headHeight + 1)
                    {
                        int head = rowIndex * columnCount + columnIndex - 1;
                        builder.Add(tail, head);
                    }
                }

                if (rowIndex < rowCount - 1)
                {
                    int headHeight = Puzzles.ParseHeight(lines[rowIndex + 1][columnIndex]);
                    if (tailHeight <= headHeight + 1)
                    {
                        int head = (rowIndex + 1) * columnCount + columnIndex;
                        builder.Add(tail, head);
                    }
                }
            }
        }

        return builder.ToGraph();
    }

    internal static bool TryFindDestination(IReadOnlyList<string> lines, out int destination)
    {
        for (int rowIndex = 0; rowIndex < lines.Count; ++rowIndex)
        {
            string row = lines[rowIndex];
            int columnIndex = row.IndexOf('E');
            if (columnIndex < 0)
                continue;

            destination = rowIndex * row.Length + columnIndex;
            return true;
        }

        destination = default;
        return false;
    }
}
