using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2022;

internal sealed class Simulator
{
    private const int Width = 7;

    private static readonly RockModel[] s_models = { Rocks.Dash, Rocks.Plus, Rocks.Corner, Rocks.Stick, Rocks.Box };

    private readonly string _jets;
    private readonly HashSet<Point> _stoppedBlocks;

    private Simulator(string jets, HashSet<Point> stoppedBlocks, int height)
    {
        Height = height;
        _jets = jets;
        _stoppedBlocks = stoppedBlocks;
    }

    internal static IReadOnlyList<RockModel> Models => s_models;

    internal int Height { get; private set; }

    internal IReadOnlySet<Point> StoppedBlocks => _stoppedBlocks;

    internal static Simulator Create(string jets, HashSet<Point> stoppedBlocks)
    {
        int height = stoppedBlocks.Count == 0 ? 0 : stoppedBlocks.Select(it => it.Y).Max() + 1;
        return new(jets, stoppedBlocks, height);
    }

    internal int Simulate(int startRockIndex, int startJetIndex, int rockCount, out int jetIndex)
    {
        int endRockIndex = startRockIndex + rockCount;
        jetIndex = startJetIndex;
        for (int rockIndex = startRockIndex; rockIndex < endRockIndex; ++rockIndex)
        {
            RockModel model = s_models[rockIndex % s_models.Length];
            Size translation = new(2, Height + 3);
            RockInstance rock = new(model, translation);
            while (true)
            {
                char jet = _jets[jetIndex++ % _jets.Length];
                if (TryPush(rock, jet, out RockInstance newRock))
                    rock = newRock;
                if (TryFall(rock, out newRock))
                    rock = newRock;
                else
                {
                    _stoppedBlocks.UnionWith(rock.GetBlocks());
                    Height = Math.Max(rock.GetMaxBound().Y + 1, Height);
                    break;
                }
            }
        }

        return Height;

        bool TryPush(RockInstance rock, char jet, out RockInstance newRock)
        {
            Size translation = jet switch
            {
                '<' => new(-1, 0),
                '>' => new(1, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(jet), jet.ToString())
            };
            newRock = new(rock.Model, rock.Translation + translation);
            if (newRock.GetMinBound().X < 0)
                return false;
            if (newRock.GetMaxBound().X >= Width)
                return false;
            return !_stoppedBlocks.Overlaps(newRock.GetBlocks());
        }

        bool TryFall(RockInstance rock, out RockInstance newRock)
        {
            newRock = new(rock.Model, rock.Translation - new Size(0, 1));
            if (newRock.GetMinBound().Y < 0)
                return false;
            return !_stoppedBlocks.Overlaps(newRock.GetBlocks());
        }
    }
}
