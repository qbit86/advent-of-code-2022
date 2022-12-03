using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo : IPuzzle<long>
{
    private const int GroupSize = 3;

    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    private long SolveCore(List<string> lines)
    {
        Span<string> rucksacks = CollectionsMarshal.AsSpan(lines);
        int groupCount = lines.Count / GroupSize;
        long result = 0L;
        for (int groupIndex = 0; groupIndex < groupCount; ++groupIndex)
        {
            int groupOffset = groupIndex * GroupSize;
            ReadOnlySpan<string> group = rucksacks.Slice(groupOffset, GroupSize);
            result += GetBadgePriority(group);
        }

        return result;
    }

    private int GetBadgePriority(ReadOnlySpan<string> rucksacks)
    {
        int rucksackCount = rucksacks.Length;
        Span<int> maskByPriority = stackalloc int[Helpers.PriorityUpperBound];
        for (int elf = 0; elf < rucksackCount; ++elf)
        {
            int mask = 1 << elf;
            string rucksack = rucksacks[elf];
            foreach (char item in rucksack)
            {
                int priority = Helpers.GetPriority(item);
                maskByPriority[priority] |= mask;
            }
        }

        int fullMask = (1 << rucksackCount) - 1;
        for (int priority = 1; priority < Helpers.PriorityUpperBound; ++priority)
        {
            if (maskByPriority[priority] == fullMask)
                return priority;
        }

        throw new UnreachableException();
    }
}
