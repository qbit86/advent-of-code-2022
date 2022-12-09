using System;
using System.Drawing;

namespace AdventOfCode2022;

internal readonly record struct Command(Size Direction, int StepCount)
{
    internal static Command Parse(string line)
    {
        Size direction = line[0] switch
        {
            'R' => new(1, 0),
            'U' => new(0, 1),
            'L' => new(-1, 0),
            'D' => new(0, -1),
            _ => throw new ArgumentException(line, nameof(line))
        };

        ReadOnlySpan<char> stepCountSpan = line.AsSpan(2);
        int stepCount = int.Parse(stepCountSpan);
        return new(direction, stepCount);
    }
}
