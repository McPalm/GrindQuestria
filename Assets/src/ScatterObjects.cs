using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterObjects : MonoBehaviour
{
    public GameObject prefab;
    public int quantity;
    public Vector2 range;
    // Start is called before the first frame update
    [Server]
    void Start()
    {
        for (int i = 0; i < quantity; i++)
        {
            var fab = Instantiate(prefab, (Vector2)transform.position - range + new Vector2(Random.value * range.x, Random.value * range.y) * 2f, Quaternion.identity, transform);
            NetworkServer.Spawn(fab);
        }
    }
}
