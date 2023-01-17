using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using Arborescence;

namespace AdventOfCode2022;

internal sealed class Graph : IForwardIncidence<Vector3, Vector3, IEnumerator<Vector3>>
{
    private readonly IReadOnlySet<Vector3> _cubes;
    private readonly Vector3 _max;
    private readonly Vector3 _min;

    public Graph(IReadOnlySet<Vector3> cubes, Vector3 min, Vector3 max)
    {
        _cubes = cubes;
        _min = min;
        _max = max;
    }

    public bool TryGetHead(Vector3 edge, [UnscopedRef] out Vector3 head) => Some(edge, out head);

    public IEnumerator<Vector3> EnumerateOutEdges(Vector3 vertex)
    {
        if (_cubes.Contains(vertex))
            yield break;

        const int maxNeighborCount = 6;
        Vector3[] neighborCandidates = ArrayPool<Vector3>.Shared.Rent(maxNeighborCount);
        try
        {
            Puzzles.PopulateNeighborCandidates(vertex, neighborCandidates);
            for (int i = 0; i < maxNeighborCount; ++i)
            {
                Vector3 neighborCandidate = neighborCandidates[i];
                if (neighborCandidate.X < _min.X || neighborCandidate.Y < _min.Y || neighborCandidate.Z < _min.Z)
                    continue;
                if (_max.X < neighborCandidate.X || _max.Y < neighborCandidate.Y || _max.Z < neighborCandidate.Z)
                    continue;
                if (_cubes.Contains(neighborCandidate))
                    continue;

                yield return neighborCandidate;
            }
        }
        finally
        {
            ArrayPool<Vector3>.Shared.Return(neighborCandidates);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Some(Vector3 valueToReturn, out Vector3 value)
    {
        value = valueToReturn;
        return true;
    }
}
