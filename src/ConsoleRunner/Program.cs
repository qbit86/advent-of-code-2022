using System;
using System.IO;
using System.Text;
using AdventOfCode2022;

try
{
    using StreamReader input = new("input.txt", Encoding.UTF8);
    PuzzlePartTwo puzzle = new(1000000000000L);
    long result = await puzzle.SolveAsync(input).ConfigureAwait(false);
    Console.WriteLine(result);
}
catch (NotImplementedException)
{
    Console.Error.WriteLine(nameof(NotImplementedException));
}
