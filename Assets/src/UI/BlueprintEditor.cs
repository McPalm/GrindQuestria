using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public TileBase[] tilePalette;
    public Hotkey1to9 hotKeys;

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (Input.GetMouseButtonDown(0))
        {
            var clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // we clicked!
            var blueprintTile = blueprint.blueprintTilemap.GetTile(clickPosition);
            var realTile = GridManager.instance.Walls.GetTile(clickPosition);
            if(realTile == Sample)
            {
                if(blueprintTile == null)
                    NetInput.SetBlueprint(blueprint.blueprintTilemap.WorldToCell(clickPosition), DeleteIndex);
                else
                    NetInput.SetBlueprint(blueprint.blueprintTilemap.WorldToCell(clickPosition), -1);
            }
            else if(realTile == null)
            {
                if(blueprintTile == Sample)
                    NetInput.SetBlueprint(blueprint.blueprintTilemap.WorldToCell(clickPosition), -1);
                else
                    NetInput.SetBlueprint(blueprint.blueprintTilemap.WorldToCell(clickPosition), SampleIndex);
            }
            else // different tile
            {
                if(blueprintTile == DeleteTile)
                    NetInput.SetBlueprint(blueprint.blueprintTilemap.WorldToCell(clickPosition), -1);
                else if(blueprintTile == Sample)
                    NetInput.SetBlueprint(blueprint.blueprintTilemap.WorldToCell(clickPosition), DeleteIndex);
                else
                    NetInput.SetBlueprint(blueprint.blueprintTilemap.WorldToCell(clickPosition), SampleIndex);
            }
        }
    }

    public void SetTileFromPalette(int paletteSlot)
    {
        if (Sample == tilePalette[paletteSlot])
            PickTile(paletteSlot);
        else
        {
            Sample = tilePalette[paletteSlot];
            for (int i = 0; i < hotKeys.buttons.Length; i++)
            {
                hotKeys.buttons[i].GetComponentsInChildren<UnityEngine.UI.Image>(true)[1].enabled = paletteSlot == i; ;
            }
        }
    }

    public void PickTile(int paletteSlot)
    {
        Building.Category category = (Building.Category)paletteSlot;

        var client = new CraftMenyClient()
        {
            CraftMenuTitle = "Building",
            buildings = BuildingCollection.Instance.GetBuildings(category),
            OnPick = SetTileInPalette,
        };
        FindObjectOfType<CraftMenuUI>().Open(client, client.PickBuilding);
    }

    public void SetTileInPalette(Building building)
    {
        int slot = (int)building.category;
        Sample = building.tile;
        tilePalette[slot] = building.tile;
        FindObjectOfType<CraftMenuUI>().Close();
        hotKeys.buttons[slot].GetComponentsInChildren<UnityEngine.UI.Image>(true)[2].sprite = building.ProductSprite;
    }

    private class CraftMenyClient : ICraftMenuClient
    {
        public string CraftMenuTitle {get; set;}
        public Building[] buildings;
        public IEnumerable<ICraftMenuItem> Items => buildings;
        public System.Action<Building> OnPick;

        public void PickBuilding(int index)
        {
            OnPick(buildings[index]);
        }
    }
}
