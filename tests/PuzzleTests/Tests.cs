using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode2022.PartTwo;

namespace AdventOfCode2022;

public sealed class Tests
{
    [Theory]
    [InlineData("sample-input.txt", 6032L)]
    [InlineData("input.txt", 31568L)]
    internal async Task PartOne(string inputPath, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var actual = await Puzzles.PartOne.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("sample-input.txt", 5031L)]
    [InlineData("input.txt", 36540L)]
    internal async Task PartTwo(string inputPath, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var actual = await Puzzles.PartTwo.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("sample-input.txt", 6, 4, 3)]
    [InlineData("sample-input-1.txt", 10, 0, 0)]
    [InlineData("sample-input-2.txt", 10, 5, 1)]
    internal async Task PartTwo_FindPositionAndFacing(
        string inputPath, int expectedPositionX, int expectedPositionY, int expectedFacing)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var (actualPosition, actualFacing) =
            await Puzzles.PartTwo.FindPositionAndFacingAsync(input).ConfigureAwait(false);
        Point expectedPosition = new(expectedPositionX, expectedPositionY);
        Assert.Equal(expectedPosition, actualPosition);

        Assert.Equal(expectedFacing, actualFacing.Mod(4));
    }

    [Fact]
    internal void PartTwo_GetPlanarPosition()
    {
        var normal = -Vector3.UnitZ;
        Point planarOrigin = new(8, 8);
        var spatialOrigin = 4f * Vector3.UnitY;
        var orientation = Vector3.UnitX;
        FaceRecord faceRecord = new(normal, planarOrigin, spatialOrigin, orientation);
        Vector3 positionOfInterest = new(1f, 2f, 0f);
        var actual = faceRecord.GetPlanarPosition(positionOfInterest);
        Point expected = new(9, 10);
        Assert.Equal(expected, actual);
    }
}
