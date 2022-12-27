namespace AdventOfCode2022;

internal interface INode
{
    ulong OpenBitset { get; }
    int TotalPressureReleased { get; }
    byte TotalFlowRate { get; }
    sbyte DepthLeft { get; }
}
