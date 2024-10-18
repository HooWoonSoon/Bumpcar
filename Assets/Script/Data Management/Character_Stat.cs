using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Stat
{
    protected string nicknames;
    protected bool isAi;
    protected int starCount;
    protected float gameDuration;
    protected int playerSpawnCount;
    protected bool isFinished;

    public Character_Stat(bool _isAI, string _nicknames)
    {
        nicknames = _nicknames;
        isAi = _isAI;
        starCount = 0;
        gameDuration = 0;
        playerSpawnCount = 0;
        isFinished = false;
    }

    public bool IsAi
    {
        get { return isAi; }
    }

    public int StarCount
    {
        get{ return starCount; }
    }

    public int PlayerSpawnCount
    {
        get { return playerSpawnCount; }
    }

    public void Finish() => isFinished = true;

    public void Death()
    {
        playerSpawnCount++;
    }

    public void Timer(float _elapseTime)
    {
        if (!isFinished)
        {
            gameDuration = _elapseTime;
        }
    }

    public void Point()
    {
        starCount++;
    }

    public override string ToString()
    {
        return $"Player:,{nicknames}, Is AI: {isAi}, Stars: {starCount}, Duration: {gameDuration}, Spawn Count: {playerSpawnCount}, Finished {isFinished}";
    }
}
