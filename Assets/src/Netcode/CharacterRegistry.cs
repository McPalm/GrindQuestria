using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRegistry : MonoBehaviour
{
    public GameObject PlayerObject;

    static public CharacterRegistry Instance { get; private set; }

    List<GameObject> UnusedCharacters = new List<GameObject>();
    List<GameObject> UsedCharacters = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetCharacter()
    {
        if(UnusedCharacters.Count > 0)
        {
            var character = UnusedCharacters[0];
            UsedCharacters.Add(character);
            UnusedCharacters.RemoveAt(0);
            return character;
        }
        else
        {
            var character = CreateCharacter();
            UsedCharacters.Add(character);
            return character;
        }
    }

    GameObject CreateCharacter()
    {
        var character = Instantiate(PlayerObject);
        NetworkServer.Spawn(character);
        return character;
    }

    public void YieldCharacter(GameObject character)
    {
        UsedCharacters.Remove(character);
        UnusedCharacters.Add(character);
    }
}
