using System;

namespace AdventOfCode2022;

internal readonly struct RockInstanceSlim : IEquatable<RockInstanceSlim>
{
    internal RockInstanceSlim(RockModelSlim model, int translation) : this(model.Data, translation) { }

    internal RockInstanceSlim(uint data, int translation)
    {
        Data = data;
        Translation = translation;
    }

    internal uint Data { get; }
    internal int Translation { get; }

    internal int LineCount()
    {
        if ((Data & 0xFF) != 0)
            return 4;
        if ((Data & 0xFF00) != 0)
            return 3;
        if ((Data & 0xFF0000) != 0)
            return 2;
        if ((Data & 0xFF000000) != 0)
            return 1;
        return 0;
    }

    public bool Equals(RockInstanceSlim other) => Data == other.Data && Translation == other.Translation;

    public override bool Equals(object? obj) => obj is RockInstanceSlim other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Data, Translation);

    public static bool operator ==(RockInstanceSlim left, RockInstanceSlim right) => left.Equals(right);

    public static bool operator !=(RockInstanceSlim left, RockInstanceSlim right) => !left.Equals(right);
}
