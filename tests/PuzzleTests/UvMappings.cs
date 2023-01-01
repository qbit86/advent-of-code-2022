using System.Collections.Generic;
using System.Numerics;
using AdventOfCode2022.PartTwo;

namespace AdventOfCode2022;

internal static class UvMappings
{
    private static UvMapping? s_sample;
    private static UvMapping? s_input;

    internal static UvMapping Sample => s_sample ??= UvMapping.CreateUnchecked(CreateSample());

    internal static UvMapping Input => s_input ??= UvMapping.CreateUnchecked(CreateInput());

    // ()()██()
    // ██████()
    // ()()████
    private static Dictionary<Vector3, FaceRecord> CreateSample() => new(6)
    {
        { Vector3.UnitZ, new(Vector3.UnitZ, new(8, 0), new(0, 0, 3), Vector3.UnitX) },
        { Vector3.UnitY, new(Vector3.UnitY, new(8, 4), new(0, 3, 3), Vector3.UnitX) },
        { -Vector3.UnitX, new(-Vector3.UnitX, new(4, 4), new(0, 0, 3), Vector3.UnitY) },
        { -Vector3.UnitY, new(-Vector3.UnitY, new(0, 4), new(3, 0, 3), -Vector3.UnitX) },
        { -Vector3.UnitZ, new(-Vector3.UnitZ, new(8, 8), new(0, 3, 0), Vector3.UnitX) },
        { Vector3.UnitX, new(Vector3.UnitX, new(12, 8), new(3, 3, 0), Vector3.UnitZ) }
    };

    // ()████
    // ()██()
    // ████()
    // ██()()
    private static Dictionary<Vector3, FaceRecord> CreateInput() => new(6)
    {
        { Vector3.UnitZ, new(Vector3.UnitZ, new(50, 0), new(0, 0, 49), Vector3.UnitX) },
        { Vector3.UnitX, new(Vector3.UnitX, new(100, 0), new(49, 0, 49), -Vector3.UnitZ) },
        { Vector3.UnitY, new(Vector3.UnitY, new(50, 50), new(0, 49, 49), Vector3.UnitX) },
        { -Vector3.UnitZ, new(-Vector3.UnitZ, new(50, 100), new(0, 49, 0), Vector3.UnitX) },
        { -Vector3.UnitX, new(-Vector3.UnitX, new(0, 100), new(0, 49, 49), -Vector3.UnitZ) },
        { -Vector3.UnitY, new(-Vector3.UnitY, new(0, 150), new(0, 0, 49), -Vector3.UnitZ) }
    };
}
