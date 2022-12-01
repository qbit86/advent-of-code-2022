using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartOne : IPuzzle<long>
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
        int[][] inventories = inventoryStrings.Select(Helpers.ParseInventory).ToArray();
        return inventories.Max(i => i.Sum());
    }
}
