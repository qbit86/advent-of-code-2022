using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Machinery;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo : IPuzzle<long>
{
    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    private static long SolveCore(IReadOnlyList<string> lines)
    {
        ProblemData problemData = new();
        StateMachine<ProblemData, string, State> parser = new(problemData, new(string.Empty));
        foreach (string line in lines)
            parser.TryProcessEvent(line);

        var directories = problemData.GetDirectories().ToList();
        IReadOnlyDictionary<string, HashSet<string>> childrenByDirectory =
            Puzzles.CreateChildrenByDirectory(directories);
        const int totalDiskSpace = 70000000;
        const int requiredSpace = 30000000;
        const int targetBound = totalDiskSpace - requiredSpace;
        int rootTotalSize = GetTotalSize("~");
        var totalSizeByDirectory = directories.ToDictionary(directory => directory, GetTotalSize);
        IEnumerable<KeyValuePair<string, int>> filtered =
            totalSizeByDirectory.Where(kv => rootTotalSize - kv.Value <= targetBound);
        int result = filtered.Select(kv => kv.Value).Min();
        return result;

        int GetTotalSize(string directory)
        {
            return problemData.GetTotalSize<HashSet<string>, IReadOnlyDictionary<string, HashSet<string>>>(
                directory, childrenByDirectory);
        }
    }
}
