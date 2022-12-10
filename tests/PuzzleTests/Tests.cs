using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class Tests
{
    [Theory]
    [InlineData("sample-input.txt", 13140L)]
    [InlineData("input.txt", 11720L)]
    internal async Task PartOne(string inputPath, long expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var actual = await Puzzles.PartOne.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("sample-input.txt",
        "##..##..##..##..##..##..##..##..##..##..###...###...###...###...###...###...###.####....####....####....####....####....#####.....#####.....#####.....#####.....######......######......######......###########.......#######.......#######.....")]
    [InlineData("input.txt",
        "####.###...##..###..####.###...##....##.#....#..#.#..#.#..#.#....#..#.#..#....#.###..#..#.#....#..#.###..#..#.#.......#.#....###..#....###..#....###..#.......#.#....#.#..#..#.#.#..#....#....#..#.#..#.####.#..#..##..#..#.####.#.....##...##..")]
    internal async Task PartTwo(string inputPath, string expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var actual = await Puzzles.PartTwo.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }
}
