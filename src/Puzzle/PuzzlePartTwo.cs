using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo : IPuzzle<long>
{
    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    private long SolveCore(IReadOnlyList<string> lines) => lines.Chunk(3).Select(GetBadgePriority).Sum();

    private int GetBadgePriority(string[] rucksacks)
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
