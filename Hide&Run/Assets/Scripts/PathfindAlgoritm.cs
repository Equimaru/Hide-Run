using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathfindAlgoritm : MonoBehaviour
{
    [SerializeField] private LayerMask unwalkableMask;

    [SerializeField] private Vector2 gridWorldSize;

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

    private void OnDrawGizmos()
    {
        if (grid == null) return;
        foreach (Node n in grid)
        {
            Gizmos.color = (n.unwalkable) ? Color.red : Color.white;
            Gizmos.DrawWireCube(n.nodeCenter, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, nodeDiameter - 0.1f));
        }
    }
}
