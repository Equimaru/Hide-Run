using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int hCost;
    public int gCost;

    public int fCost { get { return hCost + gCost; } }

    public bool unwalkable;
    public Vector3 nodeCenter;
    public int gridX, gridY;
    public Node parentNode;

    public Node(bool _unwalkable, Vector3 _nodeCenter, int _gridX, int _gridY)
    {
        unwalkable = _unwalkable;
        nodeCenter = _nodeCenter;
        gridX = _gridX;
        gridY = _gridY;
    }
    
}
