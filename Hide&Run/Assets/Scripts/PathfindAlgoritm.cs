using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathfindAlgoritm : MonoBehaviour
{
    [SerializeField] private LayerMask unwalkableMask;

    [SerializeField] private Vector2 gridWorldSize;

    [SerializeField] private Transform playerPosition;

    [SerializeField] private float nodeRadius;
    private float nodeDiameter;

    private int xSize;
    private int ySize;

    private bool unwalkable;

    private Node[,] grid;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        xSize = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        ySize = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[xSize, ySize];
        Vector3 bottomLeftPoint = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
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
                grid[x, y] = new Node(unwalkable, nodeCenter);
            }
        }
    }

    private Node GetObjectNode(Vector3 objectPosition)
    {
        float positionInPercentX = (objectPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float positionInPercentY = (objectPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        positionInPercentX = Mathf.Clamp(positionInPercentX, 0, 1);
        positionInPercentY = Mathf.Clamp(positionInPercentY, 0, 1);

        int x = Mathf.RoundToInt((gridWorldSize.x - 1) * positionInPercentX);
        int y = Mathf.RoundToInt((gridWorldSize.y - 1) * positionInPercentY);


        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        if (grid == null) return;
        Node playerNode = GetObjectNode(playerPosition.transform.position);
        foreach (Node n in grid)
        {
            Gizmos.color = (n.unwalkable) ? Color.red : Color.white;
            if (n == playerNode) Gizmos.color = Color.green;
            Gizmos.DrawWireCube(n.nodeCenter, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, nodeDiameter - 0.1f));
        }
    }
}
