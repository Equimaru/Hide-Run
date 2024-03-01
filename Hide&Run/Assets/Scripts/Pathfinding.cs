using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid grid;


    private void Awake()
    {
        grid = GetComponent<Grid>();
    }


    private void PathFind(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.GetObjectNode(startPosition);
        Node targetNode = grid.GetObjectNode(targetPosition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);




    }

    
}
