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
    public PowerType activateType { get; private set; }
    protected bool isFreeze = false;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        gameData = Game_Data.Instance;
        activateType = PowerType.None;
    }

    public void SetPlayer(int index)
    {
        characterIndex = index;
    }

    public void UpdatePosition(int currentIndex, Vector3 currentPosition, int lasWayPoint, Quaternion rotaotion)
    {
        gameData.GetIndexUpdateSpawnPosition(currentIndex, currentPosition, lasWayPoint, rotaotion);
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

    public IEnumerator PowerUpSpeed(float value, float powerTime, PowerType powerType)
    {
        Vector3 originalVelocity = drive._rigidbody.velocity;
        Vector3 originalAngularVelocity = drive._rigidbody.angularVelocity;

        activateType = powerType; 

        switch (activateType)
        {
            case PowerType.SpeedUp:
                drive._rigidbody.velocity *= value;
                break;

            case PowerType.Freeze:
                drive._rigidbody.velocity = Vector3.zero;
                drive._rigidbody.angularVelocity = Vector3.zero;
                isFreeze = true;
                break;
        }

        timer = powerTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        drive._rigidbody.velocity = originalVelocity;
        drive._rigidbody.angularVelocity = originalAngularVelocity;

        activateType = PowerType.None;
    }
}
