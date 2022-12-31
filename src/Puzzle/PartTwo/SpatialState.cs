using System.Numerics;

namespace AdventOfCode2022.PartTwo;

using Point3 = Vector3;

internal readonly record struct SpatialState(Point3 Position, Vector3 Normal, Vector3 Direction)
{
    internal SpatialState MakeTurn(int quarterTurnCount) =>
        this with { Direction = Direction.Rotate(Normal, quarterTurnCount) };

    internal SpatialState WrapAroundEdge() => this with { Normal = Direction, Direction = -Normal };

    internal SpatialState MakeStepUnchecked() => this with { Position = Position + Direction };

    internal SpatialState MakeStepUnchecked(int length) => this with { Position = Position + length * Direction };
}
