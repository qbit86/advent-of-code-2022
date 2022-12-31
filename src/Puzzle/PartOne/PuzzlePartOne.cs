using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2022.PartOne;

public sealed class PuzzlePartOne
{
    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return Solve(lines);
    }

    private static long Solve(IReadOnlyList<string> lines)
    {
        var map = Map.Create(lines);
        var instructions = Instructions.Create(lines);
        Size direction = new(1, 0);
        Point position = Puzzles.FindInitialPosition(lines);
        foreach ((char turnDirection, int stepCount) in instructions)
        {
            direction = direction.ApplyTurn(turnDirection);
            for (int i = 0; i < stepCount; ++i)
            {
                if (!map.TryApplyStep(position, direction, out Point newPosition))
                    break;
                position = newPosition;
            }
        }

        long result = ComputeFinalPassword(position, direction);
        return result;
    }

    private static long ComputeFinalPassword(Point position, Size direction)
    {
        int column = position.X + 1;
        int row = position.Y + 1;
        int facing = direction.ComputeFacing();
        return 1000 * row + 4 * column + facing;
    }
}
