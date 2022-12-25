using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class Tests
{
    [Theory]
    [InlineData("sample-input.txt", "2=-1=0")]
    [InlineData("input.txt", "20=2-02-0---02=22=21")]
    [InlineData("sample-01.txt", "1=0")]
    [InlineData("sample-02.txt", "1-0")]
    [InlineData("sample-03.txt", "1-0")]
    internal async Task PartOne(string inputPath, string expected)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        var actual = await Puzzles.PartOne.SolveAsync(input).ConfigureAwait(false);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("sample-04.txt")]
    internal async Task PartOne_Direct(string inputPath)
    {
        using StreamReader input = new(inputPath, Encoding.UTF8);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        var quinaryNumbers = lines.ToList();
        var actualInQuinary = PuzzlePartOne.SolveCore(quinaryNumbers);
        var actual = Puzzles.ConvertFromQuinary(actualInQuinary);
        var decimalNumbers = quinaryNumbers.Select(Puzzles.ConvertFromQuinary).ToList();
        var expected = decimalNumbers.Sum();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("2=-1=0", 4890)]
    [InlineData("2=-01", 976)]
    [InlineData("20", 10)]
    [InlineData("1=0", 15)]
    [InlineData("1-0", 20)]
    [InlineData("1=11-2", 2022)]
    [InlineData("1-0---0", 12345)]
    [InlineData("1121-1110-1=0", 314159265)]
    [InlineData("-1", -4)]
    [InlineData("-2", -3)]
    [InlineData("1=", 3)]
    [InlineData("1-", 4)]
    internal void FromQuinary(string numberInQuinary, int expected)
    {
        var actual = Puzzles.ConvertFromQuinary(numberInQuinary);
        Assert.Equal(expected, actual);
    }
}
