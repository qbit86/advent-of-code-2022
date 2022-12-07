namespace AdventOfCode2022;

internal static class TryHelpers
{
    internal static bool Some<T>(T valueToReturn, out T value)
    {
        value = valueToReturn;
        return true;
    }

    internal static bool None<T>(out T? value)
    {
        value = default;
        return false;
    }
}
