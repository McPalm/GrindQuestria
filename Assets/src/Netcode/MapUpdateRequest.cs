using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class MapUpdateRequest : NetworkBehaviour
{
    NetworkConnectionToClient connection;

    public HashSet<Vector3Int> Openeded = new HashSet<Vector3Int>();

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (isServer)
        {
            var identity = GetComponent<NetworkIdentity>();
            connection = identity.connectionToClient;
            yield return Check();
        }
        else
        {
            enabled = false;
        }
    }

    [Server]
    IEnumerator Check()
    {
        yield return null;
        var character = GetComponent<NetInput>().MyCharacter.transform;
        for (; ; )
        {
            foreach (var position in Around(ClosestAnchorTo(character.position)))
            {
                if (Openeded.Contains(position) == false)
                {
                    DoSync(position);
                    Openeded.Add(position);
                    yield return new WaitForSeconds(.25f);
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }

    void DoSync(Vector3Int center)
    {
        GridManager.instance.Blueprint.SendTileRequest(center, connection);
        GridManager.instance.Ground.SendTileRequest(center, connection);
        GridManager.instance.Walls.SendTileRequest(center, connection);
    }

    Vector3Int ClosestAnchorTo(Vector3 center) => new Vector3Int(Mathf.RoundToInt(center.x / 40) * 40, Mathf.RoundToInt(center.y / 40) * 40, 0);

    Vector3Int[] Around(Vector3Int center) => new Vector3Int[] {center
                                                                , center + new Vector3Int(0, 40, 0), center + new Vector3Int(40, 0, 0), center + new Vector3Int(0, -40, 0), center + new Vector3Int(-40, 0, 0)
                                                                , center + new Vector3Int(40, 40, 0), center + new Vector3Int(40, -40, 0), center + new Vector3Int(-40, 40, 0), center + new Vector3Int(-40, -40, 0) };
}
