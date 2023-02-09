using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
        var cubes = lines.Select(Puzzles.Parse).ToHashSet();
        Vector3 min = cubes.Aggregate(Vector3.Min);
        Vector3 max = cubes.Aggregate(Vector3.Max);
        Console.WriteLine($"Min: {min}");
        Console.WriteLine($"Max: {max}");
        HashSet<Vector3> boundingCells = new();
        for (int x = (int)min.X; x <= max.X; ++x)
        {
            for (int y = (int)min.Y; y <= max.Y; ++y)
            {
                boundingCells.Add(new(x, y, min.Z));
                boundingCells.Add(new(x, y, max.Z));
            }
        }

        for (int y = (int)min.Y; y <= max.Y; ++y)
        {
            for (int z = (int)min.Z; z <= max.Z; ++z)
            {
                boundingCells.Add(new(min.X, y, z));
                boundingCells.Add(new(max.X, y, z));
            }
        }

        for (int z = (int)min.Z; z <= max.Z; ++z)
        {
            for (int x = (int)min.X; x <= max.X; ++x)
            {
                boundingCells.Add(new(x, min.Y, z));
                boundingCells.Add(new(x, max.Y, z));
            }
        }

        Console.WriteLine($"{nameof(boundingCells)}: {boundingCells.Count}");
        HashSet<Vector3> exteriorCells = new();
        Graph graph = new(cubes, min, max);
        IEnumerable<Vector3> vertices = EnumerableDfs<Vector3>.EnumerateVertices(graph, boundingCells, exteriorCells);
        foreach (Vector3 _ in vertices) { }

        Console.WriteLine($"{nameof(exteriorCells)}: {exteriorCells.Count}");

        HashSet<Vector3> interiorCells = new();
        for (int x = (int)min.X; x <= max.X; ++x)
        {
            for (int y = (int)min.Y; y <= max.Y; ++y)
            {
                for (int z = (int)min.Z; z <= max.Z; ++z)
                {
                    Vector3 current = new(x, y, z);
                    if (cubes.Contains(current) || exteriorCells.Contains(current))
                        continue;

                    interiorCells.Add(current);
                }
            }
        }

        Console.WriteLine($"{nameof(interiorCells)}: {interiorCells.Count}");

        long totalSurfaceArea = Puzzles.ComputeSurfaceArea(cubes);
        long interiorSurfaceArea = Puzzles.ComputeSurfaceArea(interiorCells);
        return totalSurfaceArea - interiorSurfaceArea;
    }
}
