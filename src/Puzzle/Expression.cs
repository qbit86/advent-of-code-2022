using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using static AdventOfCode2022.TryHelpers;

namespace AdventOfCode2022;

internal abstract record Expression(string Id)
{
    internal long Evaluate(IReadOnlyDictionary<string, Expression> expressionById)
    {
        if (TryEvaluate(expressionById, out long result))
            return result;

        throw new InvalidOperationException(ToString());
    }

    internal abstract bool TryEvaluate(IReadOnlyDictionary<string, Expression> expressionById, out long value);
}

internal sealed partial record ConstantExpression(string Id, long Value) : Expression(Id)
{
    private const string Pattern = @"(\w{4}): (\d+)";

    private static readonly Lazy<Regex> s_regex = new(CreateRegex);

    private static Regex Regex => s_regex.Value;

    internal override bool TryEvaluate(IReadOnlyDictionary<string, Expression> expressionById, out long value) =>
        Some(Value, out value);

    internal static bool TryParse(string line, [NotNullWhen(true)] out ConstantExpression? value) =>
        TryParse(line, out string? id, out int number) ? Some(new(id, number), out value) : None(out value);

    private static bool TryParse(string line, [NotNullWhen(true)] out string? id, out int value)
    {
        id = default;
        value = default;
        MatchCollection matches = Regex.Matches(line);
        if (matches.Count is not 1)
            return false;
        Match match = matches[0];
        if (!match.Success)
            return false;
        GroupCollection groups = match.Groups;
        if (groups.Count != 3)
            return false;
        id = groups[1].Value;
        return int.TryParse(groups[2].ValueSpan, out value);
    }

    [GeneratedRegex(Pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex CreateRegex();
}

internal sealed record HumanVariableExpression() : Expression(HumanId)
{
    internal const string HumanId = "humn";

    internal override bool TryEvaluate(IReadOnlyDictionary<string, Expression> expressionById, out long value) =>
        None(out value);

    internal static bool TryParse(string line, [NotNullWhen(true)] out HumanVariableExpression? value) =>
        line.StartsWith(HumanId + ":") ? Some(new(), out value) : None(out value);
}

internal abstract record BinaryExpression(string Id, string Left, string Right) : Expression(Id);

internal sealed partial record OperationExpression(string Id, char Operation, string Left, string Right) :
    BinaryExpression(Id, Left, Right)
{
    private const string Pattern = @"(\w{4}): (\w{4}) (\S) (\w{4})";

    private static readonly Lazy<Regex> s_regex = new(CreateRegex);

    private long? _cache;

    private static Regex Regex => s_regex.Value;

    internal override bool TryEvaluate(IReadOnlyDictionary<string, Expression> expressionById, out long value)
    {
        if (_cache.HasValue)
            return Some(_cache.GetValueOrDefault(), out value);

        bool result = TryEvaluateCore(expressionById, out value);
        if (result)
            _cache = value;
        return result;
    }

    internal static bool TryParse(string line, [NotNullWhen(true)] out OperationExpression? value) =>
        TryParse(line, out string? id, out char operation, out string? left, out string? right)
            ? Some(new(id, operation, left, right), out value)
            : None(out value);

    private bool TryEvaluateCore(IReadOnlyDictionary<string, Expression> expressionById, out long value)
    {
        Expression leftExpression = expressionById[Left];
        if (!leftExpression.TryEvaluate(expressionById, out long leftValue))
            return None(out value);
        Expression rightExpression = expressionById[Right];
        if (!rightExpression.TryEvaluate(expressionById, out long rightValue))
            return None(out value);
        value = Operation switch
        {
            '+' => leftValue + rightValue,
            '-' => leftValue - rightValue,
            '*' => leftValue * rightValue,
            '/' => leftValue / rightValue,
            _ => throw new InvalidOperationException(ToString())
        };
        return true;
    }

    private static bool TryParse(
        string line, [NotNullWhen(true)] out string? id, out char operation,
        [NotNullWhen(true)] out string? left,
        [NotNullWhen(true)] out string? right)
    {
        id = default;
        operation = default;
        left = default;
        right = default;
        MatchCollection matches = Regex.Matches(line);
        if (matches.Count is not 1)
            return false;
        Match match = matches[0];
        if (!match.Success)
            return false;
        GroupCollection groups = match.Groups;
        if (groups.Count != 5)
            return false;
        id = groups[1].Value;
        operation = groups[3].ValueSpan[0];
        left = groups[2].Value;
        right = groups[4].Value;
        return true;
    }

    [GeneratedRegex(Pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex CreateRegex();
}

internal sealed record RootEqualityExpression(string Left, string Right) : BinaryExpression(RootId, Left, Right)
{
    internal const string RootId = "root";

    internal override bool TryEvaluate(IReadOnlyDictionary<string, Expression> expressionById, out long value) =>
        None(out value);

    internal static bool TryParse(string line, [NotNullWhen(true)] out RootEqualityExpression? value)
    {
        if (!line.StartsWith(RootId + ":"))
            return None(out value);

        string[] parts = line.Split(" ", StringSplitOptions.TrimEntries);
        if (parts.Length != 4)
            throw new ArgumentException(line, nameof(line));

        string left = parts[1];
        string right = parts[^1];
        if (left is not { Length: 4 } || right is not { Length: 4 })
            throw new ArgumentException(line, nameof(line));
        return Some(new(left, right), out value);
    }
}
