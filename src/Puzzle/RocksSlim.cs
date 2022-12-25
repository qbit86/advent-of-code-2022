namespace AdventOfCode2022;

internal static class RocksSlim
{
    internal static RockModelSlim Dash { get; } = new(0b00111100_00000000_00000000_00000000);
    internal static RockModelSlim Plus { get; } = new(0b00010000_00111000_00010000_00000000);
    internal static RockModelSlim Corner { get; } = new(0b00111000_00001000_00001000_00000000);
    internal static RockModelSlim Stick { get; } = new(0b00100000_00100000_00100000_00100000);
    internal static RockModelSlim Box { get; } = new(0b00110000_00110000_00000000_00000000);
}
