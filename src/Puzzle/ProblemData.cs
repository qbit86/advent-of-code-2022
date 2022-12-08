using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022;

internal sealed class ProblemData
{
    private readonly int[][] _heightMap;

    private ProblemData(int[][] heightMap) => _heightMap = heightMap;

    internal IReadOnlyList<int[]> HeightMap => _heightMap;

    internal static ProblemData Create(IReadOnlyList<string> lines)
    {
        int rowCount = lines.Count;
        int[][] heightMap = new int[rowCount][];
        for (int i = 0; i < rowCount; ++i)
            heightMap[i] = lines[i].Select(it => it - '0').ToArray();

        return new(heightMap);
    }
}
