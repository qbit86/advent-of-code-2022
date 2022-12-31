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
    private readonly Dictionary<Vector3, FaceRecord> _faceRecordByFaceNormal;
    private readonly string[] _tileKindByPosition;

    private Walker(string[] tileKindByPosition,
        CubeStepMaker cubeStepMaker,
        Dictionary<Vector3, FaceRecord> faceRecordByFaceNormal)
    {
        _cubeStepMaker = cubeStepMaker;
        _tileKindByPosition = tileKindByPosition;
        _faceRecordByFaceNormal = faceRecordByFaceNormal;
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

        Dictionary<Vector3, FaceRecord> faceRecordByFaceNormal =
            CreateFaceRecordByFaceNormal(faceSize, tileKindByPosition);

        return new(tileKindByPosition, cubeStepMaker, faceRecordByFaceNormal);
    }

    private static Dictionary<Vector3, FaceRecord> CreateFaceRecordByFaceNormal(
        int faceSize, IReadOnlyList<string> tileKindByPosition)
    {
        // Sample:      Input:
        // ()()██()     ()████
        // ██████()     ()██()
        // ()()████     ████()
        //              ██()()

        // TODO: Replace with actual input.
        if (faceSize == 4)
        {
            return new(6)
            {
                { Vector3.UnitZ, new(Vector3.UnitZ, new(8, 0), new(0, 0, 3), Vector3.UnitX) },
                { Vector3.UnitY, new(Vector3.UnitY, new(8, 4), new(0, 3, 3), Vector3.UnitX) },
                { -Vector3.UnitX, new(-Vector3.UnitX, new(4, 4), new(0, 0, 3), Vector3.UnitY) },
                { -Vector3.UnitY, new(-Vector3.UnitY, new(0, 4), new(3, 0, 3), -Vector3.UnitX) },
                { -Vector3.UnitZ, new(-Vector3.UnitZ, new(8, 8), new(0, 3, 0), Vector3.UnitX) },
                { Vector3.UnitX, new(Vector3.UnitX, new(12, 8), new(3, 3, 0), Vector3.UnitZ) }
            };
        }

        if (faceSize == 50)
        {
            return new(6)
            {
                { Vector3.UnitZ, new(Vector3.UnitZ, new(50, 0), new(0, 0, 49), Vector3.UnitX) },
                { Vector3.UnitX, new(Vector3.UnitX, new(100, 0), new(49, 0, 49), -Vector3.UnitZ) },
                { Vector3.UnitY, new(Vector3.UnitY, new(50, 50), new(0, 49, 49), Vector3.UnitX) },
                { -Vector3.UnitZ, new(-Vector3.UnitZ, new(50, 100), new(0, 49, 0), Vector3.UnitX) },
                { -Vector3.UnitX, new(-Vector3.UnitX, new(0, 100), new(0, 49, 49), -Vector3.UnitZ) },
                { -Vector3.UnitY, new(-Vector3.UnitY, new(0, 150), new(0, 0, 49), -Vector3.UnitZ) }
            };
        }

        PlanarState sourcePlanarState = new(Puzzles.FindInitialPosition(tileKindByPosition), 0);
        SpatialState sourceSpatialState = new((faceSize - 1) * Vector3.UnitZ, Vector3.UnitZ, Vector3.UnitX);
        Node source = new(sourcePlanarState, sourceSpatialState, Vector3.UnitX);
        HashSet<Node> explored = new(6) { source };
        Stack<Node> frontier = new(6);
        frontier.Push(source);
        while (frontier.TryPop(out Node current))
        {
            Debug.Assert(explored.Contains(current));
            (PlanarState planarState, SpatialState spatialState, Vector3 orientation) = current;
            do
            {
                PlanarState neighborPlanarStateCandidate = planarState.MakeStepUnchecked(faceSize);
                if (!IsValid(tileKindByPosition, neighborPlanarStateCandidate.Position))
                    break;
                SpatialState neighborSpatialStateCandidate = spatialState.MakeStepUnchecked(faceSize);
                if (!CubeStepMaker.IsInsideBounds(neighborSpatialStateCandidate.Position, faceSize))
                    throw new NotImplementedException();
                Node neighbor = new(planarState, spatialState, orientation);
                if (!explored.Contains(neighbor))
                {
                    explored.Add(neighbor);
                    frontier.Push(neighbor);
                }
            } while (false);
            throw new NotImplementedException();
        }

        var faceRecordByFaceNormal = explored.ToDictionary(
            it => it.SpatialState.Normal, it => it.CreateFaceRecord());
        return faceRecordByFaceNormal;
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
            FaceRecord faceRecordCandidate = _faceRecordByFaceNormal[spatialStateCandidate.Normal];
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

    private static bool IsValid(IReadOnlyList<string> tileKindByPosition, Point position)
    {
        if (unchecked((uint)position.Y >= tileKindByPosition.Count))
            return false;
        string row = tileKindByPosition[position.Y];
        if (unchecked((uint)position.X >= row.Length))
            return false;
        return row[position.X] is '.' or '#';
    }
}

internal readonly record struct Node(PlanarState PlanarState, SpatialState SpatialState, Vector3 Orientation)
{
    internal FaceRecord CreateFaceRecord() =>
        new(SpatialState.Normal, PlanarState.Position, SpatialState.Position, Orientation);
}
