using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2022;

internal struct RockInstance
{
    private Point[]? _blocks;

    internal RockInstance(RockModel model, Size translation)
    {
        Translation = translation;
        Model = model;
    }

    internal RockModel Model { get; }

    internal Size Translation { get; }

    internal readonly Point GetMinBound() => Point.Add(Model.MinBound, Translation);

    internal readonly Point GetMaxBound() => Point.Add(Model.MaxBound, Translation);

    internal IReadOnlyList<Point> GetBlocks()
    {
        _blocks ??= Model.Blocks.Select(Translate).ToArray();
        return _blocks;
    }

    private readonly Point Translate(Point position) => Point.Add(position, Translation);
}
