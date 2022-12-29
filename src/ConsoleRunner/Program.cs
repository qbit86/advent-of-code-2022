using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using AdventOfCode2022;

using StreamReader input = new("input.txt", Encoding.UTF8);
var stopwatch = Stopwatch.StartNew();
var puzzle = PuzzlePartTwo.Create(4000000);
long result = await puzzle.SolveAsync(input).ConfigureAwait(false);
Console.WriteLine(stopwatch.Elapsed);
Console.WriteLine(result);
