using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void AddPlayer(bool isAi, string nicknames, Vector3 currentSpawnPosition)
    {
        var newCharacter = new Character_Stat(isAi, nicknames, currentSpawnPosition);
        characters.Add(newCharacter);
    }

    public void CheckList()
    {
        for (int i = 0; i < characters.Count; i++) 
        {
            Debug.Log(characters[i].ToString());
        }
    }

    public void GetIndexUpdateSpawnPosition(int index, Vector3 currentPosition, int lastWayPoint, Quaternion currentRotation)
    {
        characters[index].NewSpawnPosition(currentPosition, lastWayPoint, currentRotation);
    }
    public void GetIndexUpdateTimer(int index, float time)
    {
        characters[index].Timer(time);
    }
    public void GetIndexUpdatePoint(int index)
    {
        characters[index].Point();
    }

    public void GetIndexUpdateDead(int index)
    {
        characters[index].Death();
    }
}
