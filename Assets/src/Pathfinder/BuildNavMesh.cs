using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildNavMesh : MonoBehaviour, INavMesh
{
    static readonly RaycastHit[] results = new RaycastHit[4];
    static readonly Vector3Int[] directions = new Vector3Int[] { Vector3Int.forward, Vector3Int.right, Vector3Int.back, Vector3Int.left };


    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        Build(Vector3Int.zero, new Vector3Int(64, 0, 64));
    }

    Node[,] NodeMatrix;

    public void Build(Vector3Int start, Vector3Int end)
    {
        int startX = Mathf.Min(start.x, end.x);
        int startZ = Mathf.Min(start.z, end.z);
        int sizeX = Mathf.Abs(start.x - end.x);
        int sizeZ = Mathf.Abs(start.z - end.z);
        CreateNodes(startX, startZ, sizeX, sizeZ);
        GenerateConnections();
    }

    void CreateNodes(int startX, int startZ, int sizeX, int sizeZ)
    {
        NodeMatrix = new Node[sizeX, sizeZ];
        for (int x = 0; x < sizeX; x++)
        {
            for (int z = 0; z < sizeZ; z++)
            {
                Vector3Int NodePos = new Vector3Int(startX + x, 5, startZ + z);
                var hits = Physics.RaycastNonAlloc(new Vector3(x + startX, 10, z + startZ), Vector3.down, results);
                if (hits > 0)
                {
                    NodePos.y = Mathf.RoundToInt(results[0].point.y);
                    NodeMatrix[x, z] = new Node(NodePos);
                }
            }
        }
    }

    void GenerateConnections()
    {
        List<Vector3Int> connections = new List<Vector3Int>();
        foreach(var node in NodeMatrix)
        {
            if (node == null)
                continue;
            
            foreach (var direction in BuildNavMesh.directions)
            {
                if (HasNode(node.position + direction))
                {
                    int dy = GetNode(node.position + direction).position.y - node.position.y;
                    dy = Mathf.Abs(dy);
                    if(dy < 2)
                        connections.Add(GetNode(node.position + direction).position);
                }
            }
            node.connections = connections.ToArray();
            connections.Clear();
        }
    }

    public bool HasNode(Vector3Int position)
    {
        position -= NodeMatrix[0, 0].position;
        if (position.x < 0 || position.z < 0 || position.x > NodeMatrix.GetUpperBound(0) || position.z > NodeMatrix.GetUpperBound(1))
            return false;
        return NodeMatrix[position.x, position.z] != null;
    }

    public Node GetNode(Vector3Int position) => NodeMatrix[position.x, position.z];
}
