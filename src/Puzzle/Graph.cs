using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Arborescence;
using static AdventOfCode2022.TryHelpers;

namespace AdventOfCode2022;

internal sealed class Graph : ITraversable<string, Expression, IEnumerator<Expression>>
{
    private readonly IReadOnlyDictionary<string, Expression> _expressionById;

    internal Graph(IReadOnlyDictionary<string, Expression> expressionById) => _expressionById = expressionById;

    public bool TryGetHead(Expression edge, [UnscopedRef] out string head) => Some(edge.Id, out head);

    public IEnumerator<Expression> EnumerateOutEdges(string vertex)
    {
        if (_expressionById[vertex] is not BinaryExpression binaryExpression)
            yield break;

        yield return _expressionById[binaryExpression.Left];
        yield return _expressionById[binaryExpression.Right];
    }
}
