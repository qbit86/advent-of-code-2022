using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode2022;

internal sealed record ProblemData
{
    private readonly Dictionary<string, List<File>> _filesByDirectory = new();

    internal IEnumerable<string> GetDirectories() => _filesByDirectory.Keys;

    internal bool TryGetFiles(string directory, [NotNullWhen(true)] out IReadOnlyList<File>? files) =>
        _filesByDirectory.TryGetValue(directory, out List<File>? fileList)
            ? TryHelpers.Some(fileList, out files)
            : TryHelpers.None(out files);

    internal IReadOnlyList<File> GetFiles(string directory) =>
        TryGetFiles(directory, out IReadOnlyList<File>? result) ? result : Array.Empty<File>();

    internal void AddFile(string directory, File file)
    {
        if (_filesByDirectory.TryGetValue(directory, out List<File>? fileList))
            fileList.Add(file);
        else
            _filesByDirectory.Add(directory, new() { file });
    }

    internal void AddDirectory(string directory) => _filesByDirectory.Add(directory, new());

    internal int GetTotalSize<TChildren, TChildrenByDirectory>(
        string directory, TChildrenByDirectory childrenByDirectory)
        where TChildren : IEnumerable<string>
        where TChildrenByDirectory : IReadOnlyDictionary<string, TChildren>
    {
        int fileTotalSize = GetFiles(directory).Select(file => file.Size).Sum();
        if (!childrenByDirectory.TryGetValue(directory, out TChildren? children))
            return fileTotalSize;
        return fileTotalSize + children
            .Select(child => GetTotalSize<TChildren, TChildrenByDirectory>(child, childrenByDirectory)).Sum();
    }
}
