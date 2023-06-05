using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using Arborescence;

namespace AdventOfCode2022;

internal sealed class Graph : IOutNeighborsAdjacency<Vector3, IEnumerator<Vector3>>
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

    public IEnumerator<Vector3> EnumerateOutNeighbors(Vector3 vertex)
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
}
