using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartOne : IPuzzle<string>
{
    public async Task<string> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    public static string SolveCore(IEnumerable<string> quinaryNumbers)
    {
        Queue<string> quinaryNumbersQueue = new(quinaryNumbers);
        return SolveCore(quinaryNumbersQueue);
    }

    private static string SolveCore(Queue<string> quinaryNumbers)
    {
        List<char> quinaryDigits = new();
        while (quinaryNumbers.TryDequeue(out string? leftNumber))
        {
            quinaryDigits.Clear();
            if (!quinaryNumbers.TryDequeue(out string? rightNumber))
                return leftNumber;
            int maxLength = Math.Max(leftNumber.Length, rightNumber.Length);
            for (int i = 0; i < maxLength; ++i)
            {
                Index index = ^(1 + i);
                char leftDigit = leftNumber.CharacterAtOrZero(index);
                char rightDigit = rightNumber.CharacterAtOrZero(index);
                string digitsSum = Puzzles.Add(leftDigit, rightDigit);
                quinaryDigits.Add(digitsSum[^1]);
                if (digitsSum.Length == 2)
                {
                    string carry = string.Create(i + 2, digitsSum[0], PopulateSpan);
                    quinaryNumbers.Enqueue(carry);
                }
            }

            quinaryDigits.Reverse();
            string sum = new(CollectionsMarshal.AsSpan(quinaryDigits));
            quinaryNumbers.Enqueue(sum);
        }

        throw new UnreachableException();

        static void PopulateSpan(Span<char> span, char arg)
        {
            span.Fill('0');
            span[0] = arg;
        }
    }
}
