using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool isAI;
    protected Game_Data gameData;
    public int currentIndex { get; protected set; }
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

    public void AddPoint(int currentIndex)
    {
        gameData.GetIndexUpdatePoint(currentIndex);

        gameData.CheckList();
    }
}
