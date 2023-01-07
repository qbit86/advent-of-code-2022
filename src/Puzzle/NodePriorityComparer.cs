using System.Collections.Generic;

namespace AdventOfCode2022;

internal sealed class NodePriorityComparer<TNode> : IComparer<TNode>
    where TNode : INode
{
    internal static NodePriorityComparer<TNode> Instance { get; } = new();

    public int Compare(TNode? x, TNode? y)
    {
        TNode left = x!;
        if (left.DepthLeft == 0)
            return -1;
        TNode right = y!;
        if (right.DepthLeft == 0)
            return 1;
        int guaranteedComparison =
            GuaranteedTotalPressureReleased(right).CompareTo(GuaranteedTotalPressureReleased(left));
        if (guaranteedComparison != 0)
            return guaranteedComparison;
        int openBitsetComparison = ulong.PopCount(left.OpenBitset).CompareTo(ulong.PopCount(right.OpenBitset));
        return openBitsetComparison;
    }

    private static int GuaranteedTotalPressureReleased(TNode node) =>
        node.TotalPressureReleased + node.TotalFlowRate * node.DepthLeft;
}
