using System.Drawing;

namespace AdventOfCode2022.PartTwo;

using Point2 = Point;
using Vector2 = Size;

public readonly record struct PlanarState(Point2 Position, int QuarterTurnCount)
{
    private Vector2 Direction => QuarterTurnCount.Direction();

    internal PlanarState MakeTurn(int quarterTurnCount) =>
        this with { QuarterTurnCount = QuarterTurnCount + quarterTurnCount };

    internal PlanarState MakeStepUnchecked(int length) => this with { Position = Position + length * Direction };
}
