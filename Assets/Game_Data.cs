using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Game_Data
{
    public static Game_Data Instance {  get; private set; }
    public List<Character_Stat> characters {  get; private set; }

    static Game_Data()
    {
        Instance = new Game_Data(); 
    }
    public Game_Data()
    {
        characters = new List<Character_Stat>();
    }

    public void AddPlayer(bool IsAi)
    {
        var newCharacter = new Character_Stat(IsAi);
        characters.Add(newCharacter);
    }

    public void CheckList()
    {
        for (int i = 0; i < characters.Count; i++) 
        {
            Debug.Log(characters[i].ToString() + "CurrentCharacterIndex: " + i);
        }
    }

    public void GetCharacterAtIndex(int index)
    {
        characters[index].Point();
    }
}
