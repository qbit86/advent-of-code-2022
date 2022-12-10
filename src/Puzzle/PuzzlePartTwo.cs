using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo : IPuzzle<string>
{
    private const int CrtWidth = 40;

    public async Task<string> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    public async Task SolveAndWriteAsync(TextReader input, TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        string result = SolveCore(lines);
        IEnumerable<char[]> rows = result.Chunk(CrtWidth);
        foreach (char[] row in rows)
            Console.WriteLine(row);
    }

    private static string SolveCore(IReadOnlyList<string> lines)
    {
        var commands = lines.Select(Command.Parse).ToList();
        Cpu cpu = new(commands);
        var xs = cpu.GetRegisterValues().ToList();

        return string.Create(CrtWidth * 6, xs, SpanAction);

        static void SpanAction(Span<char> pixels, List<int> xs)
        {
            for (int i = 1; i < xs.Count; ++i)
            {
                int pixelIndex = i - 1;
                pixels[pixelIndex] = Math.Abs(pixelIndex % CrtWidth - xs[i]) <= 1 ? '#' : '.';
            }
        }
    }
}
