using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

internal readonly partial record struct ValveRecord(byte Id, string Name, int Rate, IReadOnlyList<string> NeighborNames)
{
    private const RegexOptions Options =
        RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant;

    private const string Pattern =
        @"Valve (?<valve>[A-Z]{2}) has flow rate=(?<rate>\d+); tunnels? leads? to valves? (?<neighbor>[A-Z]{2})(,\s*(?<neighbor>[A-Z]{2}))*";

    private static Regex? s_regex;

    private static Regex Regex => s_regex ??= CreateRegex();

    internal static ValveRecord Parse(string line, int id)
    {
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/grouping-constructs-in-regular-expressions
        MatchCollection matches = Regex.Matches(line);
        if (matches.Count != 1)
            throw new ArgumentException(line, nameof(line));
        Match match = matches.Single();
        if (!match.Success)
            throw new ArgumentException($"{nameof(match)}.Success: false", nameof(line));
        GroupCollection groups = match.Groups;
        string name = groups["valve"].Captures.Single().Value;
        int rate = int.Parse(groups["rate"].Captures.Single().Value);
        string[] neighbors = groups["neighbor"].Captures.Select(it => it.Value).ToArray();
        return new((byte)id, name, rate, neighbors);
    }

    [GeneratedRegex(Pattern, Options)]
    private static partial Regex CreateRegex();
}
