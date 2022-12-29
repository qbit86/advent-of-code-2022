using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2022;

internal static class Extensions
{
    internal static async Task ReadAllLinesAsync<TLines>(this TextReader input, TLines lines)
        where TLines : ICollection<string>
    {
        ArgumentNullException.ThrowIfNull(input);

        while (true)
        {
            string? line = await input.ReadLineAsync().ConfigureAwait(false);
            if (line is null)
                break;

            lines.Add(line);
        }
    }

    internal static RadiusRecord Radius(this SensorRecord sensorRecord)
    {
        int distance = sensorRecord.SensorPosition.ManhattanDistance(sensorRecord.BeaconPosition);
        return new(sensorRecord.SensorPosition, distance);
    }

    internal static int ManhattanDistance(this Point position, Point other) =>
        Math.Abs(other.X - position.X) + Math.Abs(other.Y - position.Y);

    internal static Size Subtract(this Point left, Point right) => new Size(left) - new Size(right);

    internal static int Cross(this Size left, Size right) => left.Width * right.Height - left.Height * right.Width;
}
