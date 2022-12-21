using System.Numerics;

namespace AdventOfCode2022;

internal readonly record struct Vector(int Ore, int Clay, int Obsidian, int Geode) :
    IAdditionOperators<Vector, Vector, Vector>,
    ISubtractionOperators<Vector, Vector, Vector>
{
    public static Vector operator +(Vector left, Vector right) =>
        new(left.Ore + right.Ore, left.Clay + right.Clay, left.Obsidian + right.Obsidian, left.Geode + right.Geode);

    public static Vector operator -(Vector left, Vector right) =>
        new(left.Ore - right.Ore, left.Clay - right.Clay, left.Obsidian - right.Obsidian, left.Geode - right.Geode);

    internal bool IsNegativeForAnyComponent() =>
        int.IsNegative(Ore) || int.IsNegative(Clay) || int.IsNegative(Obsidian) || int.IsNegative(Geode);
}
