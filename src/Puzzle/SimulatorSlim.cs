using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AdventOfCode2022;

internal sealed class SimulatorSlim
{
    private static readonly RockModelSlim[] s_models =
        { RocksSlim.Dash, RocksSlim.Plus, RocksSlim.Corner, RocksSlim.Stick, RocksSlim.Box };

    private readonly string _jets;
    private readonly List<byte> _stoppedBlocks;

    internal SimulatorSlim(string jets, List<byte> stoppedBlocks)
    {
        _jets = jets;
        _stoppedBlocks = stoppedBlocks;
    }

    internal static int ModelCount => s_models.Length;

    internal int Height => _stoppedBlocks.Count;

    internal int Simulate(int rockIndex, int startJetIndex, out int jetIndex)
    {
        jetIndex = startJetIndex;
        RockModelSlim model = s_models[rockIndex % s_models.Length];
        RockInstanceSlim rock = new(model, Height + 3);
        while (true)
        {
            char jet = _jets[jetIndex++ % _jets.Length];
            if (TryPush(rock, jet, out RockInstanceSlim newRock))
                rock = newRock;
            if (TryFall(rock, out newRock))
                rock = newRock;
            else
            {
                Union(rock);
                break;
            }
        }

        return Height;
    }

    internal int Simulate(int startRockIndex, int startJetIndex, int rockCount, out int jetIndex)
    {
        int endRockIndex = startRockIndex + rockCount;
        jetIndex = startJetIndex;
        for (int rockIndex = startRockIndex; rockIndex < endRockIndex; ++rockIndex)
            _ = Simulate(rockIndex, jetIndex, out jetIndex);

        return Height;
    }

    private bool TryPush(RockInstanceSlim rock, char jet, out RockInstanceSlim newRock)
    {
        Span<byte> bytes = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, rock.Data);
        switch (jet)
        {
            case '<':
                ShiftLeft(bytes);
                break;
            case '>':
                ShiftRight(bytes);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(jet), jet.ToString());
        }

        uint data = BinaryPrimitives.ReadUInt32BigEndian(bytes);
        newRock = new(data, rock.Translation);
        if (uint.PopCount(rock.Data) != uint.PopCount(newRock.Data))
            return false;
        return !Overlaps(newRock);
    }

    private bool TryFall(RockInstanceSlim rock, out RockInstanceSlim newRock)
    {
        newRock = new(rock.Data, rock.Translation - 1);
        if (newRock.Translation < 0)
            return false;
        return !Overlaps(newRock);
    }

    private void Union(RockInstanceSlim rock)
    {
        int rockLineCount = rock.LineCount();
        int targetLength = rock.Translation + rockLineCount;
        while (_stoppedBlocks.Count < targetLength)
            _stoppedBlocks.Add(default);

        Span<byte> stoppedBlocks = CollectionsMarshal.AsSpan(_stoppedBlocks);
        Span<byte> topLines = stoppedBlocks[rock.Translation..];
        Span<byte> rockData = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(rockData, rock.Data);
        for (int i = 0; i < rockLineCount; ++i)
            topLines[i] = (byte)(topLines[i] | rockData[i]);
    }

    private bool Overlaps(RockInstanceSlim rock)
    {
        if (_stoppedBlocks.Count == 0 || _stoppedBlocks.Count <= rock.Translation)
            return false;
        Span<byte> stoppedBlocks = CollectionsMarshal.AsSpan(_stoppedBlocks);
        ReadOnlySpan<byte> topLines = stoppedBlocks[rock.Translation..];
        Span<byte> rockData = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(rockData, rock.Data);
        int count = Math.Min(topLines.Length, rock.LineCount());
        for (int i = 0; i < count; ++i)
        {
            if ((topLines[i] & rockData[i]) != 0)
                return true;
        }

        return false;
    }

    private static void ShiftLeft(Span<byte> bytes)
    {
        for (int i = 0; i < bytes.Length; ++i)
            bytes[i] = unchecked((byte)(bytes[i] << 1));
    }

    private static void ShiftRight(Span<byte> bytes)
    {
        for (int i = 0; i < bytes.Length; ++i)
        {
            bytes[i] = (byte)(bytes[i] >> 1);
            bytes[i] = (byte)(bytes[i] & 0b11111110);
        }
    }
}
