using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2022.PartTwo;

public readonly struct UvMapping : IReadOnlyDictionary<Vector3, FaceRecord>
{
    private readonly Dictionary<Vector3, FaceRecord> _faceRecordByFaceNormal;

    private UvMapping(Dictionary<Vector3, FaceRecord> faceRecordByFaceNormal) =>
        _faceRecordByFaceNormal = faceRecordByFaceNormal;

    public static UvMapping CreateUnchecked(Dictionary<Vector3, FaceRecord> faceRecordByFaceNormal) =>
        new(faceRecordByFaceNormal ?? throw new ArgumentNullException(nameof(faceRecordByFaceNormal)));

    public static UvMapping Create(IReadOnlyList<string> tileKindByPosition, int faceSize)
    {
        PlanarState sourcePlanarState = new(Puzzles.FindInitialPosition(tileKindByPosition), 0);
        SpatialState sourceSpatialState = new((faceSize - 1) * Vector3.UnitZ, Vector3.UnitZ, Vector3.UnitX);
        Node source = new(sourcePlanarState, sourceSpatialState, Vector3.UnitX);
        HashSet<Node> explored = new(6) { source };
        Stack<Node> frontier = new(6);
        frontier.Push(source);
        while (frontier.TryPop(out Node current))
        {
            Debug.Assert(explored.Contains(current));
            (PlanarState currentPlanarState, SpatialState currentSpatialState, Vector3 currentOrientation) = current;
            {
                PlanarState planarState = currentPlanarState;
                SpatialState spatialState = currentSpatialState;
                ProcessCurrentNode(planarState, spatialState, currentOrientation);
            }
            {
                PlanarState planarState = currentPlanarState.MakeTurn(1);
                SpatialState spatialState = currentSpatialState.MakeTurn(1);
                ProcessCurrentNode(planarState, spatialState, currentOrientation);
            }
            {
                PlanarState planarState = currentPlanarState.MakeTurn(-1);
                SpatialState spatialState = currentSpatialState.MakeTurn(-1);
                ProcessCurrentNode(planarState, spatialState, currentOrientation);
            }
        }

        var faceRecordByFaceNormal = explored.ToDictionary(
            it => it.SpatialState.Normal, it => it.CreateFaceRecord());
        return new(faceRecordByFaceNormal);

        void ProcessCurrentNode(PlanarState planarState, SpatialState spatialState, Vector3 orientation)
        {
            PlanarState neighborPlanarState = planarState.MakeStepUnchecked(faceSize);
            if (!IsValid(tileKindByPosition, neighborPlanarState.Position))
                return;
            SpatialState neighborSpatialState =
                CubeStepMaker.IsInsideBounds(spatialState.MakeStepUnchecked().Position, faceSize)
                    ? spatialState.MakeStepUnchecked(faceSize - 1).WrapAroundEdge()
                    : spatialState.WrapAroundEdge().MakeStepUnchecked(faceSize - 1);
            Debug.Assert(CubeStepMaker.IsInsideBounds(neighborSpatialState.Position, faceSize));
            Vector3 neighborOrientation = Vector3.Dot(orientation, spatialState.Direction) switch
            {
                0 => orientation,
                1 or -1 => orientation.Rotate(Vector3.Cross(spatialState.Normal, spatialState.Direction)),
                _ => throw new UnreachableException()
            };
            Node neighbor = new(neighborPlanarState, neighborSpatialState, neighborOrientation);
            if (!explored.Contains(neighbor))
            {
                explored.Add(neighbor);
                frontier.Push(neighbor);
            }
        }
    }

    #region IReadOnlyDictionary<Vector3, FaceRecord> implementation

    public IEnumerator<KeyValuePair<Vector3, FaceRecord>> GetEnumerator() => _faceRecordByFaceNormal.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _faceRecordByFaceNormal.GetEnumerator();

    public int Count => _faceRecordByFaceNormal.Count;

    public bool ContainsKey(Vector3 key) => _faceRecordByFaceNormal.ContainsKey(key);

    public bool TryGetValue(Vector3 key, out FaceRecord value) => _faceRecordByFaceNormal.TryGetValue(key, out value);

    public FaceRecord this[Vector3 key] => _faceRecordByFaceNormal[key];

    public IEnumerable<Vector3> Keys => _faceRecordByFaceNormal.Keys;

    public IEnumerable<FaceRecord> Values => _faceRecordByFaceNormal.Values;

    #endregion

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
