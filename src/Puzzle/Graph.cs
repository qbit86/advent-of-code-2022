using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Arborescence;

namespace AdventOfCode2022;

internal readonly record struct Node(Point Position, int Time);

internal sealed class Graph : IOutNeighborsAdjacency<Node, IEnumerator<Node>>
{
    private static readonly Size[] s_directions = { D.UnitX, D.UnitY, D.MinusUnitX, D.MinusUnitY, Size.Empty };
    private static readonly HashSet<Point> s_reusableSet = new();
    private readonly IReadOnlyList<Size> _directionByBlizzard;
    private readonly List<List<Point>> _positionByBlizzardByTime;

    private Graph(
        Size size, Point goal, IReadOnlyList<Size> directionByBlizzard, List<List<Point>> positionByBlizzardByTime)
    {
        Size = size;
        Goal = goal;
        _directionByBlizzard = directionByBlizzard;
        _positionByBlizzardByTime = positionByBlizzardByTime;
    }

    internal static Point Start { get; } = Point.Empty.Add(D.MinusUnitY);

    private Size Size { get; }
    internal Point Goal { get; }

    public IEnumerator<Node> EnumerateOutNeighbors(Node vertex)
    {
        (Point position, int time) = vertex;
        int newTime = time + 1;
        IEnumerable<Point> candidates = s_directions.Select(it => position.Add(it));
        return Filter(candidates, newTime);
    }

    internal static Graph Create(Size size, IReadOnlyList<Size> directionByBlizzard, List<Point> positionByBlizzard)
    {
        List<List<Point>> positionByBlizzardByTime = new() { positionByBlizzard };
        Point goal = new Point(size).Add(D.MinusUnitX);
        return new(size, goal, directionByBlizzard, positionByBlizzardByTime);
    }

    private IEnumerator<Node> Filter(IEnumerable<Point> candidates, int newTime)
    {
        IReadOnlyList<Point> positionByBlizzard = GetPositionByBlizzard(newTime);
        s_reusableSet.Clear();
        s_reusableSet.UnionWith(positionByBlizzard);
        foreach (Point candidate in candidates)
        {
            if (s_reusableSet.Contains(candidate))
                continue;
            if (candidate == Start || candidate == Goal)
            {
                yield return new(candidate, newTime);
                continue;
            }

            if (unchecked((uint)candidate.X >= (uint)Size.Width) || unchecked((uint)candidate.Y >= (uint)Size.Height))
                continue;
            yield return new(candidate, newTime);
        }
    }

    private IReadOnlyList<Point> GetPositionByBlizzard(int newTime)
    {
        if (newTime < _positionByBlizzardByTime.Count)
            return _positionByBlizzardByTime[newTime];
        if (newTime > _positionByBlizzardByTime.Count)
            throw new UnreachableException();
        IReadOnlyList<Point> positionByBlizzard = _positionByBlizzardByTime[^1];
        var newPositionByBlizzard = positionByBlizzard
            .Select((position, index) => Wrap(position.Add(_directionByBlizzard[index]))).ToList();
        _positionByBlizzardByTime.Add(newPositionByBlizzard);
        return newPositionByBlizzard;
    }

    private Point Wrap(Point position) => new(position.X.Mod(Size.Width), position.Y.Mod(Size.Height));
}
