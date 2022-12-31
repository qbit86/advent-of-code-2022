using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

internal sealed partial class Instructions
{
    private readonly List<InstructionPair> _instructionPairs;

    private Instructions(List<InstructionPair> instructionPairs) => _instructionPairs = instructionPairs;

    public IEnumerator<InstructionPair> GetEnumerator() => _instructionPairs.GetEnumerator();

    internal static Instructions Create(IReadOnlyList<string> lines)
    {
        Regex regex = CreateRegex();
        string input = lines[^1];
        MatchCollection matches = regex.Matches(input);
        Match match = matches.Single();
        if (!match.Success)
            throw new ArgumentException(input, nameof(lines));
        GroupCollection groups = match.Groups;
        if (groups.Count != 3)
            throw new ArgumentException($"{nameof(groups)}.Count: {groups.Count}", nameof(lines));
        Capture initialCapture = groups[1].Captures.Single();
        CaptureCollection restOfCaptures = groups[2].Captures;
        InstructionPair initialInstructionPair = new('_', int.Parse(initialCapture.ValueSpan));
        List<InstructionPair> instructionPairs = new(restOfCaptures.Count + 1) { initialInstructionPair };
        foreach (Capture capture in restOfCaptures)
        {
            InstructionPair instructionPair = new(capture.ValueSpan[0], int.Parse(capture.ValueSpan[1..]));
            instructionPairs.Add(instructionPair);
        }

        return new(instructionPairs);
    }

    [GeneratedRegex(@"(\d+)([LR]\d+)*", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex CreateRegex();
}

internal readonly record struct InstructionPair(char TurnDirection, int StepCount)
{
    public override string ToString() => $"{TurnDirection}{StepCount}";
}
