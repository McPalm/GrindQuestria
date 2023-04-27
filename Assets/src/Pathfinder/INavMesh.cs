using UnityEngine;

public interface INavMesh
{
    bool HasNode(Vector3Int position);
    Node GetNode(Vector3Int position);
}
