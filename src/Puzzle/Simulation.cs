using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2022;

internal sealed class Simulation
{
    private readonly HashSet<Point> _tailPositions;
    private readonly Point[] _knotPositions;

    public Simulation(int capacity, int knotCount)
    {
        _tailPositions = new(capacity) { Point.Empty };
        _knotPositions = new Point[knotCount];
    }

    internal IReadOnlySet<Point> TailPositions => _tailPositions;

    public override string ToString()
    {
        IEnumerable<string> knotPositions = _knotPositions.Select(it => it.ToString());
        return $"Count: {TailPositions.Count}, Knots: [{string.Join(", ", knotPositions)}]";
    }

    internal void Update(Command command)
    {
        for (int i = 0; i < command.StepCount; ++i)
        {
            _knotPositions[0] += command.Direction;
            for (int followerIndex = 1; followerIndex < _knotPositions.Length; ++followerIndex)
            {
                Point leader = _knotPositions[followerIndex - 1];
                ref Point follower = ref _knotPositions[followerIndex];
                Size leaderToFollowerDirection = Subtract(follower, leader);
                Size clampedDirection = Clamp(leaderToFollowerDirection);
                follower = leader + clampedDirection;
            }

            _tailPositions.Add(_knotPositions[^1]);
        }
    }

    private static Size Subtract(Point left, Point right) => new Size(left) - new Size(right);

    private static Size Clamp(Size size)
    {
        int dx = Math.Abs(size.Width);
        int dy = Math.Abs(size.Height);

        return dx.CompareTo(dy) switch
        {
            -1 => new(0, Math.Clamp(size.Height, -1, 1)),
            0 => new(Math.Clamp(size.Width, -1, 1), Math.Clamp(size.Height, -1, 1)),
            1 => new(Math.Clamp(size.Width, -1, 1), 0),
            _ => throw new UnreachableException()
        };
    }
}
