namespace AdventOfCode2022.PartOne;

internal readonly record struct Node(
    int ValveId, ulong OpenBitset, int TotalPressureReleased, byte TotalFlowRate, sbyte DepthLeft) : INode;
