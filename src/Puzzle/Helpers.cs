using System;
using System.Linq;

namespace AdventOfCode2022;

internal static class Helpers
{
    internal static readonly string[] OuterSeparators = { "\n\n", Environment.NewLine + Environment.NewLine };
    private static readonly string[] s_innerSeparators = { "\n", Environment.NewLine };

    internal static int[] ParseInventory(string inventoryString)
    {
        string[] itemStrings = inventoryString.Split(s_innerSeparators, StringSplitOptions.RemoveEmptyEntries);
        return itemStrings.Select(int.Parse).ToArray();
    }
}
