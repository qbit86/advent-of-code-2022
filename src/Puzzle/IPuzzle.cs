using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public interface IPuzzle<TResult>
{
    Task<TResult> SolveAsync(TextReader input) => Task.FromResult(Solve(input));

    TResult Solve(TextReader input) => SolveAsync(input).Result;
}
