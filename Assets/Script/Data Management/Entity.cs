using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool isAI;
    protected Game_Data gameData;
    [HideInInspector] public Wheel_Drive drive;
    public int currentIndex { get; protected set; }
    protected int characterIndex;
    public GameObject carBody;
    public float timer { get; private set; }

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

    public void UpdatePosition(int currentIndex, Vector3 currentPosition)
    {
        gameData.GetIndexUpdateSpawnPosition(currentIndex, currentPosition);
        gameData.CheckList();
    }
    public void AddPoint(int currentIndex)
    {
        gameData.GetIndexUpdatePoint(currentIndex);
        gameData.CheckList();
    }

    public void AddDead(int currentIndex)
    {
        gameData.GetIndexUpdateDead(currentIndex);
        gameData.CheckList();
    }

    public IEnumerator PowerUpSpeed(int value, float powerTime)
    {
        drive.maxSpeed *= value;
        drive.torque *= value;

        timer = powerTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        drive.maxSpeed /= value;
        drive.torque /= value;
    }
}
