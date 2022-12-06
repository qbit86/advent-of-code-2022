using System;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class TwoPointersPuzzle : IPuzzle<long>
{
    private readonly int _windowSize;

    internal TwoPointersPuzzle(int windowSize) => _windowSize = windowSize;

    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        string? line = await input.ReadLineAsync().ConfigureAwait(false);
        ArgumentException.ThrowIfNullOrEmpty(line, nameof(input));
        return SolveCore(line);
    }

    private long SolveCore(string line)
    {
        if (line.Length < _windowSize)
            throw new ArgumentException("Input was too short.", nameof(line));

        for (int start = 0, end = 1; end < line.Length; ++end)
        {
            if (end - start == _windowSize)
                return end;

            char next = line[end];
            for (int i = start; i < end; ++i)
            {
                if (line[i] == next)
                {
                    start = i + 1;
                    break;
                }
            }
        }

        throw new ArgumentException("Marker not found.", nameof(line));
    }
}
