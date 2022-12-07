using System;
using System.IO;
using Machinery;
using static AdventOfCode2022.TryHelpers;

namespace AdventOfCode2022;

internal readonly record struct State(string CurrentDir) : IState<ProblemData, string, State>
{
    public bool TryCreateNewState(ProblemData context, string ev, out State newState)
    {
        if (ParsingHelpers.TryMatchCdCommand(ev, out string? segment))
        {
            return segment switch
            {
                "/" => Some(new("~"), out newState),
                ".." => Some(new(Path.GetDirectoryName(CurrentDir)!), out newState),
                _ => Some(new(Path.Join(CurrentDir, segment)), out newState)
            };
        }

        if (ev == "$ ls")
            return None(out newState);

        if (ParsingHelpers.TryMatchFile(ev, out int size, out string? fileName))
        {
            context.AddFile(CurrentDir, new(fileName, size));
            return None(out newState);
        }

        if (ParsingHelpers.TryMatchDir(ev, out segment))
        {
            context.AddDirectory(Path.Join(CurrentDir, segment));
            return None(out newState);
        }

        throw new InvalidOperationException($"{nameof(CurrentDir)}: {CurrentDir}, ev: {ev}");
    }

    public void OnExiting(ProblemData context, string ev, State newState) { }

    public void OnRemain(ProblemData context, string ev) { }

    public void OnEntered(ProblemData context, string ev, State oldState) { }
}
