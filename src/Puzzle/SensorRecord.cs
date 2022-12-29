using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

internal readonly partial record struct SensorRecord(Point SensorPosition, Point BeaconPosition)
{
    private const RegexOptions Options =
        RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant;

    private const string Pattern =
        @"Sensor at x=(?<sensorX>-?\d+), y=(?<sensorY>-?\d+): closest beacon is at x=(?<beaconX>-?\d+), y=(?<beaconY>-?\d+)";

    private static Regex? s_regex;

    private static Regex ParsingRegex => s_regex ??= CreateRegex();

    internal static SensorRecord Parse(string line)
    {
        MatchCollection matches = ParsingRegex.Matches(line);
        Match match = matches.Single();
        GroupCollection groups = match.Groups;
        int sensorX = int.Parse(groups["sensorX"].Captures.Single().ValueSpan);
        int sensorY = int.Parse(groups["sensorY"].Captures.Single().ValueSpan);
        int beaconX = int.Parse(groups["beaconX"].Captures.Single().ValueSpan);
        int beaconY = int.Parse(groups["beaconY"].Captures.Single().ValueSpan);
        return new(new(sensorX, sensorY), new(beaconX, beaconY));
    }

    [GeneratedRegex(Pattern, Options)]
    private static partial Regex CreateRegex();
}
