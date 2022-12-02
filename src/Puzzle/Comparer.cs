using System.Collections.Generic;

namespace AdventOfCode2022;

internal sealed class Comparer : IComparer<int>
{
    internal static Comparer Instance { get; } = new();

    public int Compare(int x, int y) => 1 - (y - x + 4) % 3;
}
