using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // GridManager will serialize Node class.
public class Node
{
    public Vector2Int coordinates;
    public bool isWalkable;
    public bool isExplored;
    public bool isPath;
    public Node connectedTo;

    public Node(Vector2Int coordinates, bool isWalkable) // Constructor method
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }
}