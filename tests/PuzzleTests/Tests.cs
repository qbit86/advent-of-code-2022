using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class Tests
{
    [Theory]
    [InlineData("sample-input.txt", 10, 26L)]
    [InlineData("input.txt", 2000000, 5127797L)]
    internal async Task PartOne(string inputPath, int rowIndexOfInterest, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        PuzzlePartOne puzzle = new(rowIndexOfInterest);
        var actual = await puzzle.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("sample-input.txt", 20, 56000011L)]
    [InlineData("input.txt", 4000000, 12518502636475L)]
    internal async Task PartTwo(string inputPath, int boundInclusive, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var puzzle = PuzzlePartTwo.Create(boundInclusive);
        var actual = await puzzle.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Fact]
    internal void TryIntersect_Returns_Some()
    {
        Point leftPosition = new(2, -1);
        Size leftDirection = new(1, -1);
        Point rightPosition = new(-3, 0);
        Size rightDirection = new(1, 1);
        var hasIntersection = PuzzlePartTwo.TryIntersect(
            leftPosition, leftDirection, rightPosition, rightDirection, out var actual);
        Assert.True(hasIntersection, nameof(PuzzlePartTwo.TryIntersect) + " should return true");
        Assert.Equal(new(-1, 2), actual);
    }

    [Fact]
    internal void TryIntersect_Returns_None()
    {
        Point leftPosition = new(2, -1);
        Size leftDirection = new(1, -1);
        Point rightPosition = new(-3, 0);
        Size rightDirection = new(-1, 1);
        var hasIntersection = PuzzlePartTwo.TryIntersect(
            leftPosition, leftDirection, rightPosition, rightDirection, out var _);
        Assert.False(hasIntersection, nameof(PuzzlePartTwo.TryIntersect) + " should return false");
    }

    [Theory]
    [InlineData("sample-input.txt", 20, 14, 11)]
    internal async Task PartTwo_SingleBeacon(
        string inputPath, int boundInclusive, int expectedX, int expectedY)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var puzzle = PuzzlePartTwo.Create(boundInclusive);
        var actual = await puzzle.FindSingleBeaconAsync(input).ConfigureAwait(false);
        Assert.Equal(new(expectedX, expectedY), actual);
    }
}
