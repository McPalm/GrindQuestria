using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlueprintEditor : MonoBehaviour
{
    public Blueprint blueprint;
    public TileBase WallTile;

    public NetInput NetInput;

    int currentTile;

    void Start()
    {
        currentTile = TileCollection.Instance.IndexOf(WallTile);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // we clicked!
            var tile = blueprint.tilemap.GetTile(clickPosition);
            if (tile == null)
                NetInput.SetBlueprint(blueprint.tilemap.WorldToCell(clickPosition), currentTile);
            else
                NetInput.SetBlueprint(blueprint.tilemap.WorldToCell(clickPosition), -1);
        }
    }
}
