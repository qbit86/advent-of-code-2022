using System.Numerics;

namespace AdventOfCode2022.PartTwo;

using Point3 = Vector3;

internal readonly record struct CubeStepMaker(int UpperBoundExclusive)
{
    internal static bool IsInsideBounds(Point3 position, int upperBoundExclusive)
    {
        for (int i = 0; i < 3; ++i)
        {
            int coordinate = (int)position[i];
            if (unchecked((uint)coordinate >= upperBoundExclusive))
                return false;
        }

        return true;
    }

    internal SpatialState MakeStep(in SpatialState state) =>
        TryMakePlanarStep(state, out SpatialState result) ? result : state.WrapAroundEdge();

    private bool TryMakePlanarStep(in SpatialState state, out SpatialState result)
    {
        result = state.MakeStepUnchecked();
        return IsInsideBounds(result.Position, UpperBoundExclusive);
    }
}
