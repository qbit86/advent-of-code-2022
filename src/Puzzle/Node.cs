namespace AdventOfCode2022;

internal readonly record struct Node(
    int ElapsedMinutes,
    int OreRobotCount, int ClayRobotCount, int ObsidianRobotCount, int GeodeRobotCount,
    Vector Resources);
