using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Stat
{
    protected string nicknames;
    protected bool isAi;
    protected int starCount;
    public float gameDuration;
    protected int playerSpawnCount;
    protected bool isFinished;
    protected Vector3 lastPosition;
    protected Quaternion lastRotation;
    protected int wayPoint;

    public Character_Stat(bool _isAI, string _nicknames, Vector3 _currentCheckpoint)
    {
        nicknames = _nicknames;
        isAi = _isAI;
        lastPosition = _currentCheckpoint;
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

    public Vector3 LastPosition
    {
        get { return lastPosition; }
    }

    public Quaternion LastRotation
    {
        get { return lastRotation; }
    }

    public int WayPoint
    {
        get { return wayPoint; }
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

    public void NewSpawnPosition(Vector3 currentPosition, int lastWayPoint, Quaternion currentRotation)
    {
        lastPosition = currentPosition;
        wayPoint = lastWayPoint;
        lastRotation = currentRotation;
    }
    public void Point()
    {
        starCount++;
    }

    public override string ToString()
    {
        return $"Player:,{nicknames}, Is AI: {isAi}, Stars: {starCount}, Duration: {gameDuration}, " +
            $"Spawn Count: {playerSpawnCount}, CurrentSpawnPosition: {lastPosition} Finished {isFinished}";
    }
}
