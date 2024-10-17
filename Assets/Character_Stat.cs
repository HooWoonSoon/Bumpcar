using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Stat
{
    protected bool isAi;
    protected int starCount;
    protected float gameDuration;
    protected int playerSpawnCount;

    public Character_Stat(bool _isAI)
    {
        isAi = _isAI;
        starCount = 0;
        gameDuration = 0;
        playerSpawnCount = 0;
    }

    public void Death()
    {
        playerSpawnCount++;
    }

    public void Point()
    {
        starCount++;
    }
    public override string ToString()
    {
        return $"Is AI: {isAi}, Stars: {starCount}, Duration: {gameDuration}, Spawn Count: {playerSpawnCount}";
    }
}
