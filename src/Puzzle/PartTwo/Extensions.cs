using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace AdventOfCode2022.PartTwo;

public static class Extensions
{
    internal static Size Direction(this int quarterTurnCount) => quarterTurnCount.Mod(4) switch
    {
        0 => new SizeF(Vector2.UnitX).ToSize(),
        1 => new SizeF(Vector2.UnitY).ToSize(),
        2 => new SizeF(-Vector2.UnitX).ToSize(),
        3 => new SizeF(-Vector2.UnitY).ToSize(),
        _ => throw new UnreachableException()
    };

    internal static Vector3 Rotate(this Vector3 direction, Vector3 axis) =>
        Vector3.Cross(axis, direction);

    internal static Vector3 Rotate(this Vector3 direction, Vector3 axis, int quarterTurnCount) =>
        quarterTurnCount.Mod(4) switch
        {
            0 => direction,
            1 => direction.Rotate(axis),
            2 => -direction,
            3 => direction.Rotate(-axis),
            _ => throw new UnreachableException()
        };

    internal static int QuarterTurn(this char turnDirection) => turnDirection switch { 'L' => -1, 'R' => 1, _ => 0 };

    public static int Mod(this int dividend, int divisor) => (dividend % divisor + divisor) % divisor;
}
