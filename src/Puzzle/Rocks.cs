using System.Drawing;

namespace AdventOfCode2022;

internal static class Rocks
{
    internal static RockModel Dash { get; } =
        RockModel.Create(new[] { Point.Empty, new(1, 0), new(2, 0), new(3, 0) });

    internal static RockModel Plus { get; } =
        RockModel.Create(new Point[] { new(1, 0), new(0, 1), new(1, 1), new(2, 1), new(1, 2) });

    internal static RockModel Corner { get; } =
        RockModel.Create(new[] { Point.Empty, new(1, 0), new(2, 0), new(2, 1), new(2, 2) });

    internal static RockModel Stick { get; } =
        RockModel.Create(new[] { Point.Empty, new(0, 1), new(0, 2), new(0, 3) });

    internal static RockModel Box { get; } =
        RockModel.Create(new[] { Point.Empty, new(0, 1), new(1, 0), new(1, 1) });
}
