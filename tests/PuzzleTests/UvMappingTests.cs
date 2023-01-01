using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2022.PartTwo;

namespace AdventOfCode2022;

public sealed class UvMappingTests
{
    [Theory]
    [ClassData(typeof(UvMappingTestData))]
    internal async Task CreateAsync(string inputPath, int faceSize, UvMapping expectedMapping)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var lines = await File.ReadAllLinesAsync(inputPath, Encoding.UTF8).ConfigureAwait(false);
        var tileKindByPosition = lines.SkipLast(2).ToList();
        var expectedKeys = expectedMapping.Keys.Order(VectorComparer.Instance).ToArray();
        var actualMapping = UvMapping.Create(tileKindByPosition, faceSize);
        var actualKeys = actualMapping.Keys.Order(VectorComparer.Instance).ToArray();
        Assert.Equal(expectedKeys, actualKeys);
        foreach (var key in expectedKeys)
        {
            var expected = expectedMapping[key];
            var actual = actualMapping[key];
            Assert.Equal(expected, actual);
        }
    }
}

internal sealed class VectorComparer : IComparer<Vector3>
{
    internal static VectorComparer Instance { get; } = new();

    public int Compare(Vector3 x, Vector3 y)
    {
        var xComparison = x.X.CompareTo(y.X);
        if (xComparison != 0) return xComparison;
        var yComparison = x.Y.CompareTo(y.Y);
        if (yComparison != 0) return yComparison;
        return x.Z.CompareTo(y.Z);
    }
}
