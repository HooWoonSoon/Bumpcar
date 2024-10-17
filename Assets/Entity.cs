using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Game_Data gameData;
    protected int characterIndex;
    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        gameData = Game_Data.Instance;
    }

    public void SetPlayer(int index)
    {
        characterIndex = index;
    }

    public void AddPoint()
    {
        gameData.GetCharacterAtIndex(characterIndex);

        gameData.CheckList();
    }
}
