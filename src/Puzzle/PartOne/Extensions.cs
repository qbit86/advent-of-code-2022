using System;
using System.Drawing;

namespace AdventOfCode2022.PartOne;

internal static class Extensions
{
    internal static Size ApplyTurn(this Size direction, char turnDirection) => turnDirection switch
    {
        'L' => new(direction.Height, -direction.Width),
        'R' => new(-direction.Height, direction.Width),
        '_' => direction,
        _ => throw new ArgumentOutOfRangeException(nameof(turnDirection))
    };

    internal static int ComputeFacing(this Size direction) => direction switch
    {
        { Width: > 0, Height: 0 } => 0,
        { Width: 0, Height: > 0 } => 1,
        { Width: < 0, Height: 0 } => 2,
        { Width: 0, Height: < 0 } => 3,
        _ => throw new ArgumentException(direction.ToString(), nameof(direction))
    };
}
