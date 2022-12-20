using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AdventOfCode2022.Puzzles;

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

    private static long SolveCore(IReadOnlyList<string> lines)
    {
        int[] valueByOriginalIndex = lines.Select(int.Parse).ToArray();
        KeyValuePair<int, int>[] workingCopy =
            valueByOriginalIndex.Select((value, key) => KeyValuePair.Create(key, value)).ToArray();
        int count = valueByOriginalIndex.Length;
        int divisor = count - 1;
        for (int i = 0; i < count; ++i)
        {
            int currentValue = valueByOriginalIndex[i];
            var kv = KeyValuePair.Create(i, currentValue);
            int currentIndex = Array.IndexOf(workingCopy, kv);
            Debug.Assert(currentIndex >= 0);
            int newIndex = Mod(currentIndex + currentValue, divisor);
            if (currentIndex < newIndex)
            {
                int length = newIndex - currentIndex;
                ReadOnlySpan<KeyValuePair<int, int>> source = workingCopy.AsSpan(currentIndex + 1, length);
                Span<KeyValuePair<int, int>> destination = workingCopy.AsSpan(currentIndex, length);
                source.CopyTo(destination);
            }
            else if (newIndex < currentIndex)
            {
                int length = currentIndex - newIndex;
                ReadOnlySpan<KeyValuePair<int, int>> source = workingCopy.AsSpan(newIndex, length);
                Span<KeyValuePair<int, int>> destination = workingCopy.AsSpan(newIndex + 1, length);
                source.CopyTo(destination);
            }
            else
            {
                continue;
            }

            workingCopy[newIndex] = kv;
        }

        int[] mixedArrangement = workingCopy.Select(kv => kv.Value).ToArray();
        int indexOfZero = Array.IndexOf(mixedArrangement, 0);
        if (indexOfZero < 0)
            throw new InvalidOperationException($"{nameof(indexOfZero)} < 0");
        int wrapped1000 = Mod(indexOfZero + 1000, count);
        int wrapped2000 = Mod(indexOfZero + 2000, count);
        int wrapped3000 = Mod(indexOfZero + 3000, count);
        return mixedArrangement[wrapped1000] + mixedArrangement[wrapped2000] + mixedArrangement[wrapped3000];
    }
}
