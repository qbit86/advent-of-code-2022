using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace AdventOfCode2022;

internal sealed class ArrayComparer : IComparer<JsonArray>
{
    private ArrayComparer() { }

    internal static ArrayComparer Instance { get; } = new();

    public int Compare(JsonArray? left, JsonArray? right) => CompareArrays(left!, right!);

    private static int CompareArrays(JsonArray left, JsonArray right)
    {
        int count = Math.Min(left.Count, right.Count);
        for (int i = 0; i < count; ++i)
        {
            int result = CompareNodes(left[i]!, right[i]!);
            if (result != 0)
                return result;
        }

        return left.Count.CompareTo(right.Count);
    }

    private static int CompareNodes(JsonNode x, JsonNode y)
    {
        if (x is JsonValue left && y is JsonValue right)
            return left.GetValue<int>().CompareTo(right.GetValue<int>());

        return CompareArrays(WrapInArray(x), WrapInArray(y));
    }

    private static JsonArray WrapInArray(JsonNode node) =>
        node switch
        {
            JsonArray array => array,
            JsonValue value => new(value.GetValue<int>()),
            _ => throw new NotSupportedException(node.GetType().FullName)
        };
}
