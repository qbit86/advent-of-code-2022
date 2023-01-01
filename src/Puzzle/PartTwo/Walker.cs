using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2022.PartTwo;

internal sealed class Walker
{
    private readonly CubeStepMaker _cubeStepMaker;
    private readonly string[] _tileKindByPosition;
    private readonly UvMapping _uvMapping;

    private Walker(string[] tileKindByPosition,
        CubeStepMaker cubeStepMaker,
        UvMapping uvMapping)
    {
        _cubeStepMaker = cubeStepMaker;
        _tileKindByPosition = tileKindByPosition;
        _uvMapping = uvMapping;
    }

    internal int FaceSize => _cubeStepMaker.UpperBoundExclusive;

    internal static Walker Create(IReadOnlyList<string> lines)
    {
        if (!string.IsNullOrWhiteSpace(lines[^2]))
            throw new ArgumentException(lines[^2], nameof(lines));

        var rawTileKindByPosition = lines.SkipLast(2).ToList();
        int width = rawTileKindByPosition.Max(line => line.Length);
        string[] tileKindByPosition = rawTileKindByPosition.Select(line => line.PadRight(width, ' ')).ToArray();

        int height = tileKindByPosition.Length;
        // There are nets (unfoldings) of only 3:4 and 2:5 ratios.
        // https://upload.wikimedia.org/wikipedia/commons/thumb/c/cd/The_11_cubic_nets.svg/240px-The_11_cubic_nets.svg.png
        var gcd = BigInteger.GreatestCommonDivisor(width, height);
        int faceSize = (int)gcd;
        CubeStepMaker cubeStepMaker = new(faceSize);

        var uvMapping = UvMapping.Create(tileKindByPosition, faceSize);

        return new(tileKindByPosition, cubeStepMaker, uvMapping);
    }

    internal State TurnThenWalk(in State state, char turnDirection, int stepCount)
    {
        (PlanarState planarState, SpatialState spatialState) = state;
        (PlanarState newPlanarState, SpatialState newSpatialState) = Turn(planarState, spatialState, turnDirection);
        return Walk(newPlanarState, newSpatialState, stepCount);
    }

    private static State Turn(PlanarState planarState, SpatialState spatialState, char turnDirection)
    {
        int quarterTurn = turnDirection.QuarterTurn();
        return new(planarState.MakeTurn(quarterTurn), spatialState.MakeTurn(quarterTurn));
    }

    private State Walk(PlanarState planarState, SpatialState spatialState, int stepCount)
    {
        for (int i = 0; i < stepCount; ++i)
        {
            SpatialState spatialStateCandidate = _cubeStepMaker.MakeStep(spatialState);
            FaceRecord faceRecordCandidate = _uvMapping[spatialStateCandidate.Normal];
            Debug.Assert(faceRecordCandidate.Normal == spatialStateCandidate.Normal);
            Point planarPositionCandidate = faceRecordCandidate.GetPlanarPosition(spatialStateCandidate.Position);
            char tileKind = _tileKindByPosition[planarPositionCandidate.Y][planarPositionCandidate.X];
            if (tileKind is '#')
                break;
            Debug.Assert(tileKind is '.');
            int newQuarterTurnCount = RestoreQuarterTurnCount(
                faceRecordCandidate.Orientation, spatialStateCandidate.Direction, faceRecordCandidate.Normal);
            planarState = new(planarPositionCandidate, newQuarterTurnCount);
            spatialState = spatialStateCandidate;
        }

        return new(planarState, spatialState);
    }

    private static int RestoreQuarterTurnCount(Vector3 startDirection, Vector3 endDirection, Vector3 normal)
    {
        if (startDirection == endDirection)
            return 0;
        if (startDirection.Rotate(normal) == endDirection)
            return 1;
        if (startDirection == -endDirection)
            return 2;
        if (endDirection.Rotate(normal) == startDirection)
            return 3;
        throw new UnreachableException();
    }
}

internal readonly record struct Node(PlanarState PlanarState, SpatialState SpatialState, Vector3 Orientation)
{
    internal FaceRecord CreateFaceRecord() =>
        new(SpatialState.Normal, PlanarState.Position, SpatialState.Position, Orientation);
}
