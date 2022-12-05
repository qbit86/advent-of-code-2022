using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2022;

internal sealed class ProblemData
{
    private readonly Instruction[] _instructions;
    private readonly Stack<char>[] _stacks;

    private ProblemData(int stackCount, Stack<char>[] stacks, Instruction[] instructions)
    {
        _stacks = stacks;
        _instructions = instructions;
        StackCount = stackCount;
    }

    internal int StackCount { get; }
    internal IReadOnlyList<Stack<char>> Stacks => _stacks;
    internal IReadOnlyList<Instruction> Instructions => _instructions;

    internal static ProblemData Create(IReadOnlyList<string> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);

        var stackLines = lines.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).ToList();
        int instructionsOffset = stackLines.Count + 1;
        string footer = stackLines[^1].Trim();
        char stackCountChar = footer[^1];
        int stackCount = stackCountChar - '0';

        stackLines.RemoveAt(stackLines.Count - 1);
        var stacks = new Stack<char>[stackCount];
        for (int i = 0; i < stackCount; ++i)
        {
            Stack<char> stack = new(stackLines.Count);
            stacks[i] = stack;
            int column = 1 + i * 4;
            for (int row = stackLines.Count - 1; row >= 0; --row)
            {
                string stackLine = stackLines[row];
                if (column >= stackLine.Length)
                    break;
                char crate = stackLine[column];
                if (char.IsWhiteSpace(crate))
                    break;
                if (!char.IsAsciiLetterUpper(crate))
                    throw new InvalidOperationException(crate.ToString());
                stack.Push(crate);
            }
        }

        string[] instructionLines = lines.Skip(instructionsOffset).ToArray();
        Instruction[] instructions = instructionLines.Select(Instruction.Parse).ToArray();
        return new(stackCount, stacks, instructions);
    }
}
