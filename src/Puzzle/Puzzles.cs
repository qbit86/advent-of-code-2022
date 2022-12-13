using System.Text.Json.Nodes;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();
    public static PuzzlePartTwo PartTwo { get; } = new();

    internal static JsonArray Parse(string line) => (JsonArray)JsonNode.Parse(line)!;
}
