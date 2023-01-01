using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode2022;

public sealed class UvMappingTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "sample-input.txt", 4, UvMappings.Sample };
        yield return new object[] { "input.txt", 50, UvMappings.Input };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
