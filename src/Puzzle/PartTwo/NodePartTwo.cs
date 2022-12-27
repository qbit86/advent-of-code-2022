namespace AdventOfCode2022.PartTwo;

internal readonly record struct NodePartTwo(
    byte HumanValveId, byte ElephantValveId,
    ulong OpenBitset, int TotalPressureReleased, byte TotalFlowRate, sbyte DepthLeft) : INode;
