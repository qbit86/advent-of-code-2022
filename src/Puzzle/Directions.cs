using System.Drawing;

namespace AdventOfCode2022;

internal static class D
{
    internal static Size UnitX { get; } = new(1, 0);
    internal static Size UnitY { get; } = new(0, 1);
    internal static Size MinusUnitX { get; } = new(-1, 0);
    internal static Size MinusUnitY { get; } = new(0, -1);
}
