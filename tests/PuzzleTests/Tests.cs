using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class Tests
{
    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7L)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 5L)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 6L)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10L)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11L)]
    internal async Task PartOne_Content(string inputContent, long expected)
    {
        using StringReader input = new(inputContent);
        var actual = await Puzzles.PartOne.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("input.txt", 1235L)]
    internal async Task PartOne_Path(string inputPath, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var actual = await Puzzles.PartOne.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19L)]
    [InlineData("bvwbjplbgvbhsrlpgdmjqwftvncz", 23L)]
    [InlineData("nppdvjthqldpwncqszvftbrmjlhg", 23L)]
    [InlineData("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29L)]
    [InlineData("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26L)]
    internal async Task PartTwo_Content(string inputContent, long expected)
    {
        using StringReader input = new(inputContent);
        var actual = await Puzzles.PartTwo.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("input.txt", 3051L)]
    internal async Task PartTwo_Path(string inputPath, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var actual = await Puzzles.PartTwo.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }
}
