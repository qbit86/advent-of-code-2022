using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class Tests
{
    [Theory]
    [InlineData("sample-input.txt", 31L)]
    [InlineData("input.txt", 456L)]
    internal async Task PartOne(string inputPath, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var actual = await Puzzles.PartOne.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("sample-input.txt", 29L)]
    [InlineData("input.txt", 454L)]
    internal async Task PartTwo(string inputPath, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var actual = await Puzzles.PartTwo.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }
}
