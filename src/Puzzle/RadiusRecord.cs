using System.Drawing;

namespace AdventOfCode2022;

internal readonly record struct RadiusRecord(Point Position, int Radius)
{
    internal bool Contains(Point position) => Position.ManhattanDistance(position) <= Radius;
}
