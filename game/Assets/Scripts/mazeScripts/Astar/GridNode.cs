using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public int x;
    public int y;
    public bool isWalkable;

    public GridNode(int x, int y, bool isWalkable)
    {
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkable;
    }
}
