using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid grid;

    [SerializeField] private Transform player;
    [SerializeField] private Transform hunter;


    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void FixedUpdate()
    {
        PathFind(hunter.transform.position, player.transform.position);
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
                PathRetracing(startNode, targetNode);
                return;
            }

            foreach (Node neighbourNode in grid.GetNeighbourNodes(currentNode))
            {
                if (closedSet.Contains(neighbourNode) || neighbourNode.unwalkable == true)
                    continue;

                int movementCostToNeighbour = currentNode.gCost + GetDistanceBtwnNodes(currentNode, neighbourNode);
                if (movementCostToNeighbour < neighbourNode.gCost || !openSet.Contains(neighbourNode))
                {
                    neighbourNode.gCost = movementCostToNeighbour;
                    neighbourNode.hCost = GetDistanceBtwnNodes(neighbourNode, targetNode);
                    neighbourNode.parentNode = currentNode;

                    if (!openSet.Contains(neighbourNode))
                    {
                        openSet.Add(neighbourNode);
                    }
                }
            }
        }
    }

    private int GetDistanceBtwnNodes(Node nodeA, Node nodeB)
    {
        int distance;
        int deltaX = Mathf.Abs(nodeB.gridX - nodeA.gridX);
        int deltaY = Mathf.Abs(nodeB.gridY - nodeA.gridY);
        if (deltaX > deltaY)
        {
            distance = 10 * (deltaX - deltaY) + 14 * deltaY;
        }
        else
        {
             distance = 10 * (deltaY - deltaX) + 14 * deltaX;
        }

        return distance;
    }

    private void PathRetracing(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();

        grid.path = path;
    }

}
