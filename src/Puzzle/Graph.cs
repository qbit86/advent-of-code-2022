using System.Collections.Generic;
using Arborescence;

namespace AdventOfCode2022;

internal sealed record Graph(Blueprint Blueprint, int ElapsedMinutesBound) :
    IOutNeighborsAdjacency<Node, IEnumerator<Node>>
{
    public IEnumerator<Node> EnumerateOutNeighbors(Node vertex)
    {
        if (vertex.ElapsedMinutes == ElapsedMinutesBound)
            yield break;

        Vector currentResources = vertex.Resources;
        Vector collectedResources = new(
            vertex.OreRobotCount, vertex.ClayRobotCount, vertex.ObsidianRobotCount, vertex.GeodeRobotCount);
        if (TryCreateRobot(currentResources, Blueprint.GeodeRobotCost, out Vector remainingResources))
        {
            yield return vertex with
            {
                ElapsedMinutes = vertex.ElapsedMinutes + 1,
                GeodeRobotCount = vertex.GeodeRobotCount + 1,
                Resources = remainingResources + collectedResources
            };

            yield break;
        }

        if (TryCreateRobot(currentResources, Blueprint.ObsidianRobotCost, out remainingResources))
        {
            yield return vertex with
            {
                ElapsedMinutes = vertex.ElapsedMinutes + 1,
                ObsidianRobotCount = vertex.ObsidianRobotCount + 1,
                Resources = remainingResources + collectedResources
            };

            yield break;
        }

        if (TryCreateRobot(currentResources, Blueprint.ClayRobotCost, out remainingResources))
        {
            yield return vertex with
            {
                ElapsedMinutes = vertex.ElapsedMinutes + 1,
                ClayRobotCount = vertex.ClayRobotCount + 1,
                Resources = remainingResources + collectedResources
            };
        }

        if (TryCreateRobot(currentResources, Blueprint.OreRobotCost, out remainingResources))
        {
            yield return vertex with
            {
                ElapsedMinutes = vertex.ElapsedMinutes + 1,
                OreRobotCount = vertex.OreRobotCount + 1,
                Resources = remainingResources + collectedResources
            };
        }

        yield return vertex with
        {
            ElapsedMinutes = vertex.ElapsedMinutes + 1, Resources = currentResources + collectedResources
        };
    }

    private static bool TryCreateRobot(Vector availableResources, Vector robotCost, out Vector remainingResources)
    {
        remainingResources = availableResources - robotCost;
        return !remainingResources.IsNegativeForAnyComponent();
    }
}
