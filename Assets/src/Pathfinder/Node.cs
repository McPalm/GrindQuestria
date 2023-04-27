using UnityEngine;

public class Node
{
    public Vector3Int position;
    public Vector3Int position2D;
    public Vector3Int[] connections;

    public Node(Vector3Int position)
    {
        this.position = position;
        position2D = new Vector3Int(position.x, position.y, 0);
    }
}