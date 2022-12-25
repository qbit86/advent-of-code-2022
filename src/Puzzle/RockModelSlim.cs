using System;

namespace AdventOfCode2022;

internal readonly struct RockModelSlim : IEquatable<RockModelSlim>
{
    internal RockModelSlim(uint data) => Data = data;

    internal uint Data { get; }

    public bool Equals(RockModelSlim other) => Data == other.Data;

    public override bool Equals(object? obj) => obj is RockModelSlim other && Equals(other);

    public override int GetHashCode() => (int)Data;

    public static bool operator ==(RockModelSlim left, RockModelSlim right) => left.Equals(right);

    public static bool operator !=(RockModelSlim left, RockModelSlim right) => !left.Equals(right);
}
