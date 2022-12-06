using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class SlowPuzzle : IPuzzle<long>
{
    private readonly int _windowSize;

    internal SlowPuzzle(int windowSize) => _windowSize = windowSize;

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

        int windowCount = line.Length - _windowSize + 1;
        for (int i = 0; i < windowCount; ++i)
        {
            int distinctCount = line.Substring(i, _windowSize).Distinct().Count();
            if (distinctCount == _windowSize)
                return i + _windowSize;
        }

        throw new ArgumentException("Marker not found.", nameof(line));
    }
}
