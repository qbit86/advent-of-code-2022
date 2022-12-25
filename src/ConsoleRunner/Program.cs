using System;
using System.IO;
using System.Text;
using AdventOfCode2022;

using StreamReader input = new("input.txt", Encoding.UTF8);
long result = await Puzzles.PartOne.SolveAsync(input).ConfigureAwait(false);
Console.WriteLine(result);
