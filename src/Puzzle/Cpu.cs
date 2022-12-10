using System;
using System.Collections.Generic;

namespace AdventOfCode2022;

internal sealed class Cpu
{
    internal Cpu(IReadOnlyList<Command> commands) => Commands = commands;

    private IReadOnlyList<Command> Commands { get; }

    internal IEnumerable<int> GetRegisterValues()
    {
        int x = 1;
        yield return x;
        foreach (Command command in Commands)
        {
            switch (command.Kind)
            {
                case CommandKind.Noop:
                    yield return x;
                    break;
                case CommandKind.Addx:
                    yield return x;
                    yield return x;
                    x += command.Argument;
                    break;
                default:
                    throw new InvalidOperationException(command.ToString());
            }
        }
    }
}
