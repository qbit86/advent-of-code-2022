using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartOne : IPuzzle<long>
{
    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    private long SolveCore(IReadOnlyList<string> lines) => lines.Select(GetPriorityOfOddItem).Sum();

    private int GetPriorityOfOddItem(string rucksack)
    {
        int rucksackLength = rucksack.Length;
        Span<int> priorities = stackalloc int[rucksackLength];
        for (int i = 0; i < rucksackLength; ++i)
            priorities[i] = Helpers.GetPriority(rucksack[i]);

        int leftLength = rucksackLength >> 1;
        ReadOnlySpan<int> leftCompartment = priorities[..leftLength];
        ReadOnlySpan<int> rightCompartment = priorities[leftLength..];
        Span<bool> presenceByPriority = stackalloc bool[Helpers.PriorityUpperBound];
        foreach (int itemPriority in leftCompartment)
        {
            Debug.Assert(itemPriority is >= 0 and < Helpers.PriorityUpperBound);
            presenceByPriority[itemPriority] = true;
        }

        foreach (int itemPriority in rightCompartment)
        {
            if (presenceByPriority[itemPriority])
                return itemPriority;
        }

        throw new UnreachableException(rucksack);
    }
}
