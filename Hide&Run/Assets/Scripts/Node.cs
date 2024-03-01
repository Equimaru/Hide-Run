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

    public Node(bool _unwalkable, Vector3 _nodeCenter)
    {
        unwalkable = _unwalkable;
        nodeCenter = _nodeCenter;
    }
    
}
