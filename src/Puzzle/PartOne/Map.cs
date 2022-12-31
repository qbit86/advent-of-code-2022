using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode2022.PartOne;

internal sealed class Map
{
    private static readonly char[] s_validTileKinds = { '.', '#' };
    private readonly string[] _tileKindByPosition;

    private Map(string[] tileKindByPosition) => _tileKindByPosition = tileKindByPosition;

    internal IReadOnlyList<string> TileKindByPosition => _tileKindByPosition;

    internal static Map Create(IReadOnlyList<string> lines)
    {
        if (!string.IsNullOrWhiteSpace(lines[^2]))
            throw new ArgumentException(lines[^2], nameof(lines));

        string[] tileKindByPosition = lines.SkipLast(2).ToArray();
        return new(tileKindByPosition);
    }

    internal bool TryApplyStep(Point position, Size direction, out Point finalPosition)
    {
        var candidate = Point.Add(position, direction);
        char candidateKind = GetTileKind(candidate);
        return candidateKind switch
        {
            '.' => Some(candidate, out finalPosition),
            '#' => None(out finalPosition),
            ' ' => TryWrap(out finalPosition),
            _ => throw new InvalidOperationException(candidateKind.ToString())
        };

        bool TryWrap(out Point f)
        {
            return direction switch
            {
                { Width: not 0, Height: 0 } => TryWrapHorizontal(out f),
                { Width: 0, Height: not 0 } => TryWrapVertical(out f),
                _ => throw new InvalidOperationException(direction.ToString())
            };
        }

        bool TryWrapHorizontal(out Point f)
        {
            int rowIndex = position.Y;
            string row = _tileKindByPosition[rowIndex];
            int columnIndexWrapped = direction.Width > 0
                ? row.IndexOfAny(s_validTileKinds)
                : row.LastIndexOfAny(s_validTileKinds);
            char kind = row[columnIndexWrapped];
            return kind switch
            {
                '.' => Some(new(columnIndexWrapped, rowIndex), out f),
                '#' => None(out f),
                _ => throw new UnreachableException(kind.ToString())
            };
        }

        bool TryWrapVertical(out Point f)
        {
            int columnIndex = position.X;
            int rowIndexWrapped = direction.Height > 0
                ? Array.FindIndex(_tileKindByPosition,
                    row => columnIndex < row.Length && s_validTileKinds.Contains(row[columnIndex]))
                : Array.FindLastIndex(_tileKindByPosition,
                    row => columnIndex < row.Length && s_validTileKinds.Contains(row[columnIndex]));
            char kind = _tileKindByPosition[rowIndexWrapped][columnIndex];
            return kind switch
            {
                '.' => Some(new(columnIndex, rowIndexWrapped), out f),
                '#' => None(out f),
                _ => throw new UnreachableException(kind.ToString())
            };
        }
    }

    private char GetTileKind(Point position) => GetTileKind(position.Y, position.X);

    private char GetTileKind(int rowIndex, int columnIndex)
    {
        if (unchecked((uint)rowIndex >= (uint)_tileKindByPosition.Length))
            return ' ';
        string row = _tileKindByPosition[rowIndex];
        if (unchecked((uint)columnIndex >= (uint)row.Length))
            return ' ';
        return row[columnIndex];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Some(Point valueToReturn, out Point value)
    {
        value = valueToReturn;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool None(out Point value)
    {
        value = default;
        return false;
    }
}
