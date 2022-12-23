using System;
using System.Collections.Generic;
using System.Drawing;
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
        HashSet<Point> elfPositions = Puzzles.CreateElfPositions(lines);
        _ = Puzzles.Simulate(elfPositions).Take(10).Last();

        Point min = elfPositions.Aggregate(Min);
        Point max = elfPositions.Aggregate(Max);
        long aabbArea = (max.X - min.X + 1L) * (max.Y - min.Y + 1L);
        long result = aabbArea - elfPositions.Count;
        return result;

        static Point Min(Point left, Point right)
        {
            int x = Math.Min(left.X, right.X);
            int y = Math.Min(left.Y, right.Y);
            return new(x, y);
        }

        static Point Max(Point left, Point right)
        {
            int x = Math.Max(left.X, right.X);
            int y = Math.Max(left.Y, right.Y);
            return new(x, y);
        }
    }
}
