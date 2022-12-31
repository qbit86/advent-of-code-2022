using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;

namespace AdventOfCode2022.PartTwo;

public sealed class PuzzlePartTwo
{
    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return Solve(lines);
    }

    public async Task<PlanarState> FindPositionAndFacingAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return FindPositionAndFacing(lines);
    }

    private static PlanarState FindPositionAndFacing(IReadOnlyList<string> lines)
    {
        var walker = Walker.Create(lines);
        var instructions = Instructions.Create(lines);

        Point initialPosition = Puzzles.FindInitialPosition(lines);
        PlanarState initialPlanarState = new(initialPosition, 0);
        SpatialState initialSpatialState = new((walker.FaceSize - 1) * Vector3.UnitZ, Vector3.UnitZ, Vector3.UnitX);
        State state = new(initialPlanarState, initialSpatialState);
        foreach ((char turnDirection, int stepCount) in instructions)
            state = walker.TurnThenWalk(state, turnDirection, stepCount);

        return state.Planar;
    }

    private static long Solve(IReadOnlyList<string> lines)
    {
        (Point finalPosition, int finalQuarterTurnCount) = FindPositionAndFacing(lines);
        long result = ComputeFinalPassword(finalPosition, finalQuarterTurnCount);
        return result;
    }

    private static long ComputeFinalPassword(Point position, int quarterTurnCount)
    {
        // Let's avoid using X and Y components directly.
        Vector2 vector = new SizeF(position).ToVector2() + Vector2.One;
        float dotProduct = Vector2.Dot(vector, new(4f, 1000f));
        return (int)dotProduct + quarterTurnCount.Mod(4);
    }
}
