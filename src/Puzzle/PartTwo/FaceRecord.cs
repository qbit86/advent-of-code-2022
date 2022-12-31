using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace AdventOfCode2022.PartTwo;

using Point2 = Point;
using Point3 = Vector3;
using Vector2 = Size;

public readonly record struct FaceRecord(
    Vector3 Normal, Point2 PlanarOrigin, Point3 SpatialOrigin, Vector3 Orientation)
{
    public Point2 GetPlanarPosition(Point3 position)
    {
        Vector3 spatialOffset = position - SpatialOrigin;
        Vector3 localY = Orientation.Rotate(Normal);
        int x = (int)Vector3.Dot(spatialOffset, Orientation);
        Debug.Assert(x >= 0);
        int y = (int)Vector3.Dot(spatialOffset, localY);
        Debug.Assert(y >= 0);
        Vector2 planarOffset = new(x, y);
        return PlanarOrigin + planarOffset;
    }
}
