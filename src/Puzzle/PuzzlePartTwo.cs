using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arborescence.Traversal;

namespace AdventOfCode2022;

public sealed class PuzzlePartTwo : IPuzzle<long>
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
        Task future1 = DumpAsync(expressions,
            $"{expressions.Length}-{expressions.OfType<BinaryExpression>().Count()}-{expressions.Length}");
        var expressionById = expressions.ToDictionary(it => it.Id);
        foreach (Expression current in expressions)
        {
            if (current.TryEvaluate(expressionById, out long value))
                expressionById[current.Id] = new ConstantExpression(current.Id, value);
        }

        var refinedExpressions = expressionById.Values.ToList();
        Task future2 = DumpAsync(refinedExpressions,
            $"{expressions.Length}-{refinedExpressions.OfType<BinaryExpression>().Count()}-{refinedExpressions.Count}");

        EnumerableDfs<Graph, string, Expression, IEnumerator<Expression>> dfs = new();
        Graph graph = new(expressionById);
        HashSet<string> exploredSet = new();
        IEnumerator<string> vertexEnumerator = dfs.EnumerateVertices(graph, RootEqualityExpression.RootId, exploredSet);
        while (vertexEnumerator.MoveNext()) { }

        foreach (Expression current in refinedExpressions)
        {
            if (!exploredSet.Contains(current.Id))
                expressionById.Remove(current.Id);
        }

        var moreRefinedExpressions = expressionById.Values.ToList();
        Task future3 = DumpAsync(moreRefinedExpressions,
            $"{expressions.Length}-{moreRefinedExpressions.OfType<BinaryExpression>().Count()}-{moreRefinedExpressions.Count}");

        Task.WaitAll(future1, future2, future3);

        var root = (RootEqualityExpression)expressionById[RootEqualityExpression.RootId];
        Expression leftExpression = expressionById[root.Left];
        Expression rightExpression = expressionById[root.Right];
        if (leftExpression.TryEvaluate(expressionById, out long leftValue))
            return Restore(expressionById, rightExpression, leftValue);
        if (rightExpression.TryEvaluate(expressionById, out long rightValue))
            return Restore(expressionById, leftExpression, rightValue);

        throw new UnreachableException(root.ToString());
    }

    private static Expression Parse(string line)
    {
        if (HumanVariableExpression.TryParse(line, out HumanVariableExpression? humanVariableExpression))
            return humanVariableExpression;
        if (RootEqualityExpression.TryParse(line, out RootEqualityExpression? rootEqualityExpression))
            return rootEqualityExpression;
        if (ConstantExpression.TryParse(line, out ConstantExpression? constantExpression))
            return constantExpression;
        if (OperationExpression.TryParse(line, out OperationExpression? operationExpression))
            return operationExpression;
        throw new ArgumentException(line, nameof(line));
    }

    private static async Task DumpAsync(IReadOnlyList<Expression> expressions, string name)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        string directory = Path.Join(Path.GetTempPath(), "__" + today.ToString("O"));
        Directory.CreateDirectory(directory);
        string path = Path.Join(directory, $"{name}.gv");
        await Console.Out.WriteLineAsync(path).ConfigureAwait(false);
        await using var writer = new StreamWriter(path, false, Encoding.UTF8);
        await writer.WriteLineAsync($"digraph \"{name}\" {{").ConfigureAwait(false);
        try
        {
            await writer.WriteLineAsync("  node [shape=record]").ConfigureAwait(false);
            StringBuilder sb = new();
            foreach (Expression current in expressions)
            {
                sb.Clear();
                sb.Append($"  {current.Id} [");
                sb.Append($"label = \"{current.Id} | {Caption(current)}\"");
                if (current.Id is HumanVariableExpression.HumanId or RootEqualityExpression.RootId)
                    sb.Append(" style=filled");
                sb.Append(']');
                await writer.WriteLineAsync(sb.ToString()).ConfigureAwait(false);
            }

            var binaryExpressions = expressions.OfType<BinaryExpression>().ToList();
            foreach (BinaryExpression current in binaryExpressions)
            {
                await writer.WriteLineAsync($"  {current.Id} -> {current.Left}").ConfigureAwait(false);
                await writer.WriteLineAsync($"  {current.Id} -> {current.Right}").ConfigureAwait(false);
            }
        }
        finally
        {
            await writer.WriteLineAsync("}").ConfigureAwait(false);
        }

        static string Caption(Expression expression)
        {
            return expression switch
            {
                ConstantExpression c => c.Value.ToString(),
                HumanVariableExpression => "x",
                OperationExpression o => o.Operation.ToString(),
                RootEqualityExpression => "=",
                _ => "?"
            };
        }
    }

    private static long Restore(
        IReadOnlyDictionary<string, Expression> expressionById, Expression expression, long expectedValue)
    {
        if (expression is HumanVariableExpression)
            return expectedValue;

        if (expression is not OperationExpression operationExpression)
            throw new InvalidOperationException(expression.ToString());

        Expression leftExpression = expressionById[operationExpression.Left];
        Expression rightExpression = expressionById[operationExpression.Right];

        if (leftExpression.TryEvaluate(expressionById, out long leftValue))
        {
            return operationExpression.Operation switch
            {
                '+' => Restore(expressionById, rightExpression, expectedValue - leftValue),
                '*' => Restore(expressionById, rightExpression, expectedValue / leftValue),
                '/' => Restore(expressionById, rightExpression, leftValue / expectedValue),
                '-' => Restore(expressionById, rightExpression, leftValue - expectedValue),
                _ => throw new InvalidOperationException(operationExpression.ToString())
            };
        }

        if (rightExpression.TryEvaluate(expressionById, out long rightValue))
        {
            return operationExpression.Operation switch
            {
                '+' => Restore(expressionById, leftExpression, expectedValue - rightValue),
                '*' => Restore(expressionById, leftExpression, expectedValue / rightValue),
                '/' => Restore(expressionById, leftExpression, expectedValue * rightValue),
                '-' => Restore(expressionById, leftExpression, expectedValue + rightValue),
                _ => throw new InvalidOperationException(operationExpression.ToString())
            };
        }

        throw new UnreachableException(expression.ToString());
    }
}
