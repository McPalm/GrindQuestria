using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinder : MonoBehaviour
{
    public INavMesh NavMesh;

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        if (!NavMesh.HasNode(goal))
            throw new System.Exception("Goal does not exist in the grid");
        var openSet = new List<Vector3Int>();
        var closedSet = new HashSet<Vector3Int>();
        var GValue = new Dictionary<Vector3Int, int>(); // lowest cost to get there
        int GValueAt(Vector3Int v3) => GValue.ContainsKey(v3) ? GValue[v3] : int.MaxValue;
        float HValueAt(Vector3Int v3) => GValueAt(v3) + ((Vector3)v3 - goal).magnitude;

        void Sort() => openSet.Sort((a, b) => System.Math.Sign(HValueAt(a) - HValueAt(b)));

        openSet.Add(start);
        GValue.Add(start, 0);
        while(openSet.Count > 0)
        {
            var current = openSet[0];
            // Debug.Log($"Trying {current}");
            openSet.RemoveAt(0);
            closedSet.Add(current);
            foreach(var connection in NavMesh.GetNode(current).connections)
            {
                if (closedSet.Contains(connection))
                    continue;
                if (openSet.Contains(connection))
                    continue;

                openSet.Add(connection);
                int climb = (connection.y - NavMesh.GetNode(current).position.y);
                climb = Mathf.Clamp(climb, 0, 1);
                
                GValue.Add(connection, GValueAt(current) + 1 + climb);
                if(connection == goal)
                {
                    goto buildPath;
                }
            }
            if (openSet.Count + closedSet.Count > 1000)
                break;
            Sort();
        }
        Debug.Log("Failed at finding path");
        return null;

        buildPath:

        Vector3Int BestConnection(Vector3Int v3)
        {
            var bestValue = int.MaxValue;
            var bestConnection = Vector3Int.zero;

            foreach (var connection in NavMesh.GetNode(v3).connections)
            {
                if (GValueAt(connection) < bestValue)
                {
                    bestConnection = connection;
                    bestValue = GValueAt(connection);
                }
            }
            return bestConnection;
        }

        var nodes = new List<Vector3Int> { goal };
        int sanity = GValueAt(goal) + 4;
        while (nodes[0] != start && sanity-- > 0)
            nodes.Insert(0, BestConnection(nodes[0]));

        return nodes;
    }
}
