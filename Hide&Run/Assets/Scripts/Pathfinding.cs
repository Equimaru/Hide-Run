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

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (currentNode.fCost > openSet[i].fCost || currentNode.fCost == openSet[i].fCost && currentNode.hCost > openSet[i].hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return;
            }

            foreach (Node neighourNode in grid.GetNeighbourNodes(currentNode))
            {
                if (openSet.Contains(neighourNode) || neighourNode.unwalkable == true)
                    continue;


            }

        }


    }

    
}
