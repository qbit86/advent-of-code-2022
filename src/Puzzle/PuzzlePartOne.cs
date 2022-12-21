using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartOne : IPuzzle<long>
{
    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        List<string> lines = new();
        await input.ReadAllLinesAsync(lines).ConfigureAwait(false);
        return SolveCore(lines);
    }

    private static long SolveCore(IReadOnlyList<string> lines)
    {
        Expression[] expressions = lines.Select(Parse).ToArray();
        var expressionById = expressions.ToDictionary(it => it.Id);
        long result = expressionById["root"].Evaluate(expressionById);
        return result;
    }

    private static Expression Parse(string line)
    {
        if (ConstantExpression.TryParse(line, out ConstantExpression? constantExpression))
            return constantExpression;
        if (OperationExpression.TryParse(line, out OperationExpression? operationExpression))
            return operationExpression;
        throw new ArgumentException(line, nameof(line));
    }
}
