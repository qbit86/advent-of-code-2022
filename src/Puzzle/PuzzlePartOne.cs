using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public sealed class PuzzlePartOne : IPuzzle<long>
{
    private const int RoundCount = 20;

    public async Task<long> SolveAsync(TextReader input)
    {
        ArgumentNullException.ThrowIfNull(input);
        string inputContent = await input.ReadToEndAsync().ConfigureAwait(false);
        return SolveCore(inputContent);
    }

    private static long SolveCore(string input)
    {
        Monkey[] monkeys = Monkey.ParseAll(input);
        int monkeyCount = monkeys.Length;
        long[] inspectCountByMonkey = new long[monkeyCount];
        for (int roundIndex = 0; roundIndex < RoundCount; ++roundIndex)
        {
            for (int monkeyId = 0; monkeyId < monkeyCount; ++monkeyId)
            {
                Monkey monkey = monkeys[monkeyId];
                while (monkey.WorryLevels.TryDequeue(out long worryLevel))
                {
                    inspectCountByMonkey[monkeyId] += 1;
                    long newWorryLevel = monkey.UpdateWorryLevel(worryLevel);
                    newWorryLevel /= 3;
                    int destinationId = monkey.ChooseDestination(newWorryLevel);
                    monkeys[destinationId].WorryLevels.Enqueue(newWorryLevel);
                }
            }
        }

        var topActivities = inspectCountByMonkey.OrderDescending().Take(2).ToList();
        return topActivities.Aggregate(1L, (left, right) => left * right);
    }
}
