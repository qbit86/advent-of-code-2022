using System;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

internal sealed partial record Blueprint(
    int Id, Vector OreRobotCost, Vector ClayRobotCost, Vector ObsidianRobotCost, Vector GeodeRobotCost)
{
    private const string Pattern =
        @"Blueprint (\d+):\s+Each ore robot costs (\d+) ore.\s+Each clay robot costs (\d+) ore.\s+Each obsidian robot costs (\d+) ore and (\d+) clay.\s+Each geode robot costs (\d+) ore and (\d+) obsidian.";

    private static readonly Lazy<Regex> s_regex = new(CreateRegex);

    internal static Blueprint Parse(string line)
    {
        MatchCollection matches = s_regex.Value.Matches(line);
        if (matches.Count > 1)
            throw new ArgumentException($"{nameof(matches)}.Count: {matches.Count}", nameof(line));
        if (matches[0] is not { Success: true } match)
            throw new ArgumentException("Success: false", nameof(line));
        GroupCollection groups = match.Groups;
        if (groups.Count != 8)
            throw new ArgumentException($"{nameof(groups)}.Count: {groups.Count}", nameof(line));
        int id = int.Parse(groups[1].ValueSpan);
        Vector oreRobotCost = new(int.Parse(groups[2].ValueSpan), 0, 0, 0);
        Vector clayRobotCost = new(int.Parse(groups[3].ValueSpan), 0, 0, 0);
        Vector obsidianRobotCost = new(int.Parse(groups[4].ValueSpan), int.Parse(groups[5].ValueSpan), 0, 0);
        Vector geodeRobotCost = new(int.Parse(groups[6].ValueSpan), 0, int.Parse(groups[7].ValueSpan), 0);
        return new(id, oreRobotCost, clayRobotCost, obsidianRobotCost, geodeRobotCost);
    }

    [GeneratedRegex(Pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex CreateRegex();
}
