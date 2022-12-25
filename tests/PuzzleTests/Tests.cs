using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class Tests
{
    private const long LargeRockCount = 1000000000000L;

    [Theory]
    [InlineData("sample-input.txt", 1, 1L)]
    [InlineData("sample-input.txt", 2, 4L)]
    [InlineData("sample-input.txt", 3, 6L)]
    [InlineData("sample-input.txt", 4, 7L)]
    [InlineData("sample-input.txt", 5, 9L)]
    [InlineData("sample-input.txt", 6, 10L)]
    [InlineData("sample-input.txt", 7, 13L)]
    [InlineData("sample-input.txt", 8, 15L)]
    [InlineData("sample-input.txt", 9, 17L)]
    [InlineData("sample-input.txt", 10, 17L)]
    [InlineData("sample-input.txt", 2022, 3068L)]
    [InlineData("input.txt", 2022, 3102L)]
    internal async Task PartOne(string inputPath, int rockCount, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        PuzzlePartOne puzzle = new(rockCount);
        var actual = await puzzle.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("sample-input.txt", 1L, 1L)]
    [InlineData("sample-input.txt", 2L, 4L)]
    [InlineData("sample-input.txt", 3L, 6L)]
    [InlineData("sample-input.txt", 4L, 7L)]
    [InlineData("sample-input.txt", 5L, 9L)]
    [InlineData("sample-input.txt", 6L, 10L)]
    [InlineData("sample-input.txt", 7L, 13L)]
    [InlineData("sample-input.txt", 8L, 15L)]
    [InlineData("sample-input.txt", 9L, 17L)]
    [InlineData("sample-input.txt", 10L, 17L)]
    [InlineData("sample-input.txt", 2022L, 3068L)]
    [InlineData("sample-input.txt", LargeRockCount, 1514285714288L)]
    [InlineData("input.txt", 2022L, 3102L)]
    [InlineData("input.txt", LargeRockCount, 1539823008825L)]
    internal async Task PartTwo(string inputPath, long rockCount, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        PuzzlePartTwo puzzle = new(rockCount);
        var actual = await puzzle.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }
}
