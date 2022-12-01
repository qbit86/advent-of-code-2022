using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo : IPuzzle<long>
{
    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        string text = await input.ReadToEndAsync().ConfigureAwait(false);
        return SolveCore(text);
    }

    private long SolveCore(string text)
    {
        string[] inventoryStrings = text.Split(Helpers.OuterSeparators, StringSplitOptions.RemoveEmptyEntries);
        IEnumerable<int> sums = inventoryStrings.Select(Helpers.ParseInventory).Select(inventory => inventory.Sum());
        // return sums.OrderDescending().Take(3).Sum();
        // https://en.wikipedia.org/wiki/Selection_algorithm#Unordered_partial_sorting
        // Sorting is too boring, Quickselect is too cumbersome, so let's use quaternary min-heap.
        IEnumerable<(int, int)> totals = sums.Select(t => (t, t));
        PriorityQueue<int, int> queue = new(totals, Comparer.Instance);
        long result = 0L;
        for (int i = 0; i < 3; ++i)
            result += queue.Dequeue();
        return result;
    }
}

internal sealed class Comparer : IComparer<int>
{
    internal static Comparer Instance { get; } = new();

    public int Compare(int x, int y) => y.CompareTo(x);
}
