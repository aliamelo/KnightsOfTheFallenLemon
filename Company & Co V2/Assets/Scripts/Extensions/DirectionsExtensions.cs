using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionsExtensions
{
    //S'utilise comme t1.GetDirection(t2)
    public static Direction GetDirection (this Tile t1, Tile t2)
    {
        if (t1.pos.y < t2.pos.y)
            return Direction.North;

        if (t1.pos.x < t2.pos.x)
            return Direction.East;

        if (t1.pos.y > t2.pos.y)
            return Direction.South;

        return Direction.West;
    }

    public static Vector3 ToEuler (this Direction d)
    {
        return new Vector3(0, (int)d * 90, 0);
    }
}
