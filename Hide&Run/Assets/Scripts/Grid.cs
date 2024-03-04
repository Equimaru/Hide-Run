using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private LayerMask unwalkableMask;

    [SerializeField] private Vector2 gridWorldSize;

    [SerializeField] private Transform playerPosition;

    [SerializeField] private float nodeRadius;
    private float nodeDiameter;

    private int gridSizeX;
    private int gridSizeY;

    private bool unwalkable;

    private Node[,] grid;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeftPoint = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 nodeCenter = bottomLeftPoint + Vector3.right * (nodeDiameter * x + nodeRadius) + Vector3.forward * (nodeDiameter * y + nodeRadius);
                if (Physics.CheckBox(nodeCenter, new Vector3(nodeRadius, nodeRadius, nodeRadius), Quaternion.identity, unwalkableMask))
                {
                    unwalkable = true;
                }
                else
                {
                    unwalkable = false;
                }
                grid[x, y] = new Node(unwalkable, nodeCenter, x, y);
            }
        }
    }

    public List<Node> GetNeighbourNodes(Node node)
    {
        List<Node> neighbourNodes = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                
                if (node.gridX + x >= 0 && node.gridX + x < gridSizeX && node.gridY + y >= 0 && node.gridY + y < gridSizeY) // =< or < with gridWorldSize
                {
                    neighbourNodes.Add(grid[node.gridX + x, node.gridY + y]);
                }
            }
        }
        return neighbourNodes;
    }

    public Node GetObjectNode(Vector3 objectPosition)
    {
        float positionInPercentX = (objectPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float positionInPercentY = (objectPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        positionInPercentX = Mathf.Clamp(positionInPercentX, 0, 1);
        positionInPercentY = Mathf.Clamp(positionInPercentY, 0, 1);

        int x = Mathf.RoundToInt((gridWorldSize.x - 1) * positionInPercentX);
        int y = Mathf.RoundToInt((gridWorldSize.y - 1) * positionInPercentY);


        return grid[x, y];
    }

    public List<Node> path;

    private void OnDrawGizmos()
    {
        if (grid == null) return;
        Node playerNode = GetObjectNode(playerPosition.transform.position);
        foreach (Node n in grid)
        {
            Gizmos.color = (n.unwalkable) ? Color.red : Color.white;
            if (path != null)
            {
                if (path.Contains(n)) Gizmos.color = Color.blue;
            }
            if (n == playerNode) Gizmos.color = Color.green;
            Gizmos.DrawWireCube(n.nodeCenter, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, nodeDiameter - 0.1f));
        }
    }
}
