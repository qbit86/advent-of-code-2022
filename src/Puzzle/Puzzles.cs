using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static IReadOnlyDictionary<string, HashSet<string>> CreateChildrenByDirectory(
        ICollection<string> directories)
    {
        Dictionary<string, HashSet<string>> childrenByDirectory = new(directories.Count);
        PopulateChildrenByDirectory(directories, childrenByDirectory);
        return childrenByDirectory;
    }

    private static void PopulateChildrenByDirectory(
        IEnumerable<string> directories, IDictionary<string, HashSet<string>> childrenByDirectory)
    {
        foreach (string directory in directories)
        {
            if (Path.GetDirectoryName(directory) is not { Length: > 0 } parent)
                continue;

            if (childrenByDirectory.TryGetValue(parent, out HashSet<string>? children))
                children.Add(directory);
            else
                childrenByDirectory.Add(parent, new() { directory });
        }
    }
}
