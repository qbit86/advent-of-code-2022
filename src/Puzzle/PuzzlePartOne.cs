using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        var problemData = ProblemData.Create(lines);
        IReadOnlyList<int[]> heightMap = problemData.HeightMap;
        int rowCount = heightMap.Count;
        int result = 0;
        for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
        {
            int[] row = heightMap[rowIndex];
            for (int columnIndex = 0; columnIndex < row.Length; ++columnIndex)
            {
                int treeHeight = row[columnIndex];
                var column = Enumerable.Range(0, rowCount).Select(i => heightMap[i][columnIndex]).ToList();
                IEnumerable<int>[] rays =
                {
                    row.Take(columnIndex),
                    row.Skip(columnIndex + 1),
                    column.Take(rowIndex),
                    column.Skip(rowIndex + 1)
                };
                bool isVisible = rays.Any(ray => ray.All(it => it < treeHeight));
                if (isVisible)
                    ++result;
            }
        }

        return result;
    }
}
