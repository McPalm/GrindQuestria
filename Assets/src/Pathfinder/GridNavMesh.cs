using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridNavMesh : MonoBehaviour, INavMesh
{
    // editor
    public Tilemap walls;

    // private
    Dictionary<Vector3Int, Node> nodes = new Dictionary<Vector3Int, Node>();

    public Node GetNode(Vector3Int position)
    {
        if (!nodes.ContainsKey(position))
            nodes.Add(position, new Node(position));
        Node ret = nodes[position];
        var connections = new List<Vector3Int>();
        if (IsAccessible(position + Vector3Int.up))
            connections.Add(position + Vector3Int.up);
        if (IsAccessible(position + Vector3Int.right))
            connections.Add(position + Vector3Int.right);
        if (IsAccessible(position + Vector3Int.down))
            connections.Add(position + Vector3Int.down);
        if (IsAccessible(position + Vector3Int.left))
            connections.Add(position + Vector3Int.left);
        ret.connections = connections.ToArray();
        return ret;
    }

    public bool HasNode(Vector3Int position)
    {
        return IsAccessible(position);
    }

    bool IsAccessible(Vector3Int position)
    {
        var tile = walls.GetTile(position);
        if (tile == null)
            return true;
        if(tile is BuildingTile)
        {
            return ((BuildingTile)tile).colliderType == Tile.ColliderType.None;
        }
        return false;
    }
}
