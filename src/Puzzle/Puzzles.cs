using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static Vector3 Parse(string line)
    {
        string[] parts = line.Split(',');
        if (parts.Length != 3)
            throw new ArgumentException($"{nameof(parts)}.Length: {parts.Length}", nameof(line));
        int x = int.Parse(parts[0], CultureInfo.InvariantCulture);
        int y = int.Parse(parts[1], CultureInfo.InvariantCulture);
        int z = int.Parse(parts[2], CultureInfo.InvariantCulture);
        return new(x, y, z);
    }

    internal static void PopulateNeighborCandidates(Vector3 vertex, Span<Vector3> neighborCandidates)
    {
        neighborCandidates[0] = vertex + Vector3.UnitX;
        neighborCandidates[1] = vertex + Vector3.UnitY;
        neighborCandidates[2] = vertex + Vector3.UnitZ;
        neighborCandidates[3] = vertex - Vector3.UnitX;
        neighborCandidates[4] = vertex - Vector3.UnitY;
        neighborCandidates[5] = vertex - Vector3.UnitZ;
    }

    internal static long ComputeSurfaceArea(IReadOnlySet<Vector3> cubes)
    {
        long result = 0L;
        Span<Vector3> neighborCandidates = stackalloc Vector3[6];
        foreach (Vector3 cube in cubes)
        {
            PopulateNeighborCandidates(cube, neighborCandidates);
            int sidesExposed = 6;
            foreach (Vector3 candidate in neighborCandidates)
            {
                if (cubes.Contains(candidate))
                    --sidesExposed;
            }

            result += sidesExposed;
        }

        return result;
    }
}
