using System;
using System.IO;
using System.Text;
using AdventOfCode2022;

#if false
try
{
    using StreamReader input = new("input.txt", Encoding.UTF8);
    long result = await Puzzles.PartOne.SolveAsync(input).ConfigureAwait(false);
    Console.WriteLine(result);
}
catch (NotImplementedException)
{
    Console.Error.WriteLine(nameof(NotImplementedException));
}
#endif

try
{
    using StreamReader input = new("input.txt", Encoding.UTF8);
    long result = await Puzzles.PartTwo.SolveAsync(input).ConfigureAwait(false);
    Console.WriteLine(result);
}
catch (NotImplementedException)
{
    Console.Error.WriteLine(nameof(NotImplementedException));
}
