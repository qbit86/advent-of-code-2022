using System;
using System.Diagnostics;

namespace AdventOfCode2022;

public static class Puzzles
{
    public static PuzzlePartOne PartOne { get; } = new();

    public static long ConvertFromQuinary(string numberInQuinary)
    {
        long result = 0L;
        long power = 1L;
        for (int i = numberInQuinary.Length - 1; i >= 0; --i)
        {
            char quinaryDigit = numberInQuinary[i];
            int decimalRepresentation = ConvertFromQuinary(quinaryDigit);
            result += power * decimalRepresentation;
            power *= 5L;
        }

        return result;
    }

    internal static string Add(char left, char right)
    {
        int leftNumber = ConvertFromQuinary(left);
        int rightNumber = ConvertFromQuinary(right);
        int sum = leftNumber + rightNumber;
        string result = sum switch
        {
            -4 => "-1",
            -3 => "-2",
            -2 => "=",
            -1 => "-",
            0 => "0",
            1 => "1",
            2 => "2",
            3 => "1=",
            4 => "1-",
            _ => throw new UnreachableException()
        };
        return result;
    }

    private static int ConvertFromQuinary(char quinaryDigit) => quinaryDigit switch
    {
        '2' => 2, '1' => 1, '0' => 0, '-' => -1, '=' => -2,
        _ => throw new ArgumentOutOfRangeException(nameof(quinaryDigit), quinaryDigit, null)
    };
}
