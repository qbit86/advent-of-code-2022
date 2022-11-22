using System.Text;

namespace AdventOfCode2022;

public sealed class Tests
{
    [Theory]
    [InlineData("sample-input.txt", 0L)]
    internal async Task PartOne(string inputPath, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var actual = await Puzzles.PartOne.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Theory(Skip = "Not implemented yet")]
    [InlineData("sample-input.txt", 0L)]
    internal async Task PartTwo(string inputPath, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var actual = await Puzzles.PartTwo.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }
}
