using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2022;

#if AOC_INPUT_EXAMPLE
Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3
#endif

internal sealed class Monkey
{
    private static readonly string[] s_outerSeparators = { "\n\n", Environment.NewLine + Environment.NewLine };
    private static readonly string[] s_innerSeparators = { "\n", Environment.NewLine };

    private Monkey(int id, Queue<long> worryLevels, char operation, int argument,
        int divisor, int trueDestination, int falseDestination)
    {
        WorryLevels = worryLevels;
        Id = id;
        Operation = operation;
        Argument = argument;
        Divisor = divisor;
        TrueDestination = trueDestination;
        FalseDestination = falseDestination;
    }

    internal int Id { get; }

    internal Queue<long> WorryLevels { get; }
    internal char Operation { get; }
    internal int Argument { get; }
    internal int Divisor { get; }
    internal int TrueDestination { get; }
    internal int FalseDestination { get; }

    internal static Monkey[] ParseAll(string notes) =>
        notes.Split(s_outerSeparators, StringSplitOptions.TrimEntries).Select(Parse).ToArray();

    internal static Monkey Parse(string note, int expectedId)
    {
        string[] lines = note.Split(s_innerSeparators, StringSplitOptions.TrimEntries);
        int id = ParseId(lines[0]);
        if (id != expectedId)
            throw new ArgumentException(lines[0], nameof(note));
        long[] startingItems = ParseStartingItems(lines[1]);
        Queue<long> worryLevels = new(startingItems);
        (char operation, int argument) = ParseOperationAndArgument(lines[2]);
        int testDivisor = ParseDivisor(lines[3]);
        int trueDestination = ParseTrueDestination(lines[4]);
        int falseDestination = ParseFalseDestination(lines[5]);
        return new(id, worryLevels, operation, argument, testDivisor, trueDestination, falseDestination);
    }

    internal long UpdateWorryLevel(long worryLevel) =>
        Operation switch
        {
            '²' => worryLevel * worryLevel,
            '+' => worryLevel + Argument,
            '*' => worryLevel * Argument,
            _ => throw new UnreachableException($"{Operation} {Argument}")
        };

    internal int ChooseDestination(long worryLevel) => worryLevel % Divisor == 0 ? TrueDestination : FalseDestination;

    private static int ParseId(string line)
    {
        int endExclusive = line.LastIndexOf(':');
        if (endExclusive < 0)
            throw new ArgumentException(line, nameof(line));
        const string prefix = "Monkey ";
        if (!line.StartsWith(prefix))
            throw new ArgumentException(line, nameof(line));
        return int.Parse(line.AsSpan(prefix.Length, endExclusive - prefix.Length));
    }

    private static long[] ParseStartingItems(string line)
    {
        const string prefix = "Starting items: ";
        if (!line.StartsWith(prefix))
            throw new ArgumentException(line, nameof(line));
        return line[prefix.Length..].Split(',', StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();
    }

    private static (char, int) ParseOperationAndArgument(string line)
    {
        const string prefix = "Operation: new = old ";
        if (!line.StartsWith(prefix))
            throw new ArgumentException(line, nameof(line));
        ReadOnlySpan<char> operationAndArgumentSpan = line.AsSpan(prefix.Length);
        char operation = operationAndArgumentSpan[0];
        if (operation is not '+' and not '*')
            throw new ArgumentException(line, nameof(line));
        ReadOnlySpan<char> argumentSpan = operationAndArgumentSpan[2..];
        if (int.TryParse(argumentSpan, out int argument))
            return (operation, argument);
        if (argumentSpan is not "old")
            throw new NotSupportedException(new(operationAndArgumentSpan));

        return operation switch
        {
            '*' => ('²', default),
            _ => throw new NotSupportedException(new(operationAndArgumentSpan))
        };
    }

    private static int ParseDivisor(string line)
    {
        const string prefix = "Test: divisible by ";
        if (!line.StartsWith(prefix))
            throw new ArgumentException(line, nameof(line));
        return int.Parse(line.AsSpan(prefix.Length));
    }

    private static int ParseTrueDestination(string line)
    {
        const string prefix = "If true: throw to monkey ";
        if (!line.StartsWith(prefix))
            throw new ArgumentException(line, nameof(line));
        return int.Parse(line.AsSpan(prefix.Length));
    }

    private static int ParseFalseDestination(string line)
    {
        const string prefix = "If false: throw to monkey ";
        if (!line.StartsWith(prefix))
            throw new ArgumentException(line, nameof(line));
        return int.Parse(line.AsSpan(prefix.Length));
    }
}
