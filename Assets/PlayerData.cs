using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData 
{
    public string name;
    public bool isAi;
    public float timer;
    public int pointStar;
    public int respawnTime;

    public PlayerData(string _name, bool _isAi)
    {
        this.name = _name;
        this.isAi = _isAi;
        pointStar = 0;
        respawnTime = 0;
        timer = 0;
    }

    public void IncreasePoint(bool Activate)
    {
        pointStar++;
    }

    public void RespawnCount(bool Trigger)
    {
        respawnTime++;
    }
}