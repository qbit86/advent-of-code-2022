using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Machinery;

namespace AdventOfCode2022;

public sealed class PuzzlePartOne : IPuzzle<long>
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

        int result = directories.Select(GetTotalSize).Where(totalSize => totalSize <= 100000).Sum();
        return result;

        int GetTotalSize(string directory)
        {
            return problemData.GetTotalSize<HashSet<string>, IReadOnlyDictionary<string, HashSet<string>>>(
                directory, childrenByDirectory);
        }
    }
}
