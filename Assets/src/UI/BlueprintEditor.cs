using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlueprintEditor : MonoBehaviour
{
    public Blueprint blueprint;
    [SerializeField]
    public TileBase Sample;
    public TileBase DeleteTile;
    public NetInput NetInput;

    int SampleIndex => TileCollection.Instance.IndexOf(Sample);
    int DeleteIndex => TileCollection.Instance.IndexOf(DeleteTile);

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // we clicked!
            var blueprintTile = blueprint.tilemap.GetTile(clickPosition);
            var realTile = GridManager.instance.Walls.GetTile(clickPosition);
            if(realTile == Sample)
            {
                if(blueprintTile == null)
                    NetInput.SetBlueprint(blueprint.tilemap.WorldToCell(clickPosition), DeleteIndex);
                else
                    NetInput.SetBlueprint(blueprint.tilemap.WorldToCell(clickPosition), -1);
            }
            else if(realTile == null)
            {
                if(blueprintTile == Sample)
                    NetInput.SetBlueprint(blueprint.tilemap.WorldToCell(clickPosition), -1);
                else
                    NetInput.SetBlueprint(blueprint.tilemap.WorldToCell(clickPosition), SampleIndex);
            }
            else // different tile
            {
                if(blueprintTile == DeleteTile)
                    NetInput.SetBlueprint(blueprint.tilemap.WorldToCell(clickPosition), -1);
                else if(blueprintTile == Sample)
                    NetInput.SetBlueprint(blueprint.tilemap.WorldToCell(clickPosition), DeleteIndex);
                else
                    NetInput.SetBlueprint(blueprint.tilemap.WorldToCell(clickPosition), SampleIndex);
            }
        }
    }
}
