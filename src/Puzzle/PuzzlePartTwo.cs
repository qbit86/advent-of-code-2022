using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        var problemData = ProblemData.Create(lines);
        IReadOnlyList<int[]> heightMap = problemData.HeightMap;
        int rowCount = heightMap.Count;
        long result = long.MinValue;
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            int[] row = heightMap[rowIndex];
            for (int columnIndex = 0; columnIndex < row.Length; ++columnIndex)
            {
                int treeHeight = row[columnIndex];
                var column = Enumerable.Range(0, rowCount).Select(i => heightMap[i][columnIndex]).ToList();
                List<int>[] rays =
                {
                    row.Take(columnIndex).Reverse().ToList(),
                    row.Skip(columnIndex + 1).ToList(),
                    column.Take(rowIndex).Reverse().ToList(),
                    column.Skip(rowIndex + 1).ToList()
                };

                IEnumerable<int> viewingDistances = rays.Select(ray => ComputeViewingDistance(treeHeight, ray));

                long score = viewingDistances.Aggregate(1L, (left, right) => left * right);
                if (score > result)
                    result = score;
            }
        }

        return result;

        static int ComputeViewingDistance(int treeHeight, List<int> ray)
        {
            if (ray.Count == 0)
                return 0;

            int index = ray.FindIndex(it => it >= treeHeight);
            return index < 0 ? ray.Count : index + 1;
        }
    }
}
