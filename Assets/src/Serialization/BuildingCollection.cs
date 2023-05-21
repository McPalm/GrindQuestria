using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[CreateAssetMenu]
public class BuildingCollection : ScriptableObjectCollection<Building>
{
    BuildingCollection instance;
    public BuildingCollection Instance
    {
        get
        {
            if(instance == null)
            {
                instance = Resources.Load<BuildingCollection>(typeof(BuildingCollection).ToString());
                instance.Init();
            }
            return instance;
        }
    }

    Building[] Walls;
    Building[] Floors;
    Building[] Doors;
    Building[] Shops;
    Building[] Furniture;

    void Init()
    {
        Walls = List.Where(b => b.category == Building.Category.wall).ToArray();
        Floors = List.Where(b => b.category == Building.Category.floor).ToArray();
        Doors = List.Where(b => b.category == Building.Category.door).ToArray();
        Shops = List.Where(b => b.category == Building.Category.shop).ToArray();
        Furniture = List.Where(b => b.category == Building.Category.furniture).ToArray();
    }
}
