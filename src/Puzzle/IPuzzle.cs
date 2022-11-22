using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2022;

public interface IPuzzle<TResult>
{
    Task<TResult> SolveAsync(TextReader input);
}
