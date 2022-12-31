using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using AdventOfCode2022;

using StreamReader input = new("input.txt", Encoding.UTF8);
var stopwatch = Stopwatch.StartNew();
long result = await Puzzles.PartTwo.SolveAsync(input).ConfigureAwait(false);
Console.WriteLine(stopwatch.Elapsed);
Console.WriteLine(result);
