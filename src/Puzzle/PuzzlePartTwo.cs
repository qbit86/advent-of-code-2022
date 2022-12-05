using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo : IPuzzle<string>
{
    public async Task<string> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    private static string SolveCore(IReadOnlyList<string> lines)
    {
        var problemData = ProblemData.Create(lines);
        foreach (Instruction instruction in problemData.Instructions)
        {
            int count = instruction.Count;
            Stack<char> from = problemData.Stacks[instruction.From];
            Stack<char> to = problemData.Stacks[instruction.To];
            Stack<char> temp = new(count);
            for (int i = 0; i < count; ++i)
            {
                char crate = from.Pop();
                temp.Push(crate);
            }

            for (int i = 0; i < count; ++i)
            {
                char crate = temp.Pop();
                to.Push(crate);
            }
        }

        return string.Create(problemData.StackCount, problemData, SpanAction);

        static void SpanAction(Span<char> span, ProblemData data)
        {
            for (int i = 0; i < data.StackCount; ++i)
                span[i] = data.Stacks[i].Peek();
        }
    }
}
