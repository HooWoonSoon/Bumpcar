using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public List<Entity> entity;
    public float globalTimer;

    private void Awake()
    {
        foreach (Entity go in Resources.FindObjectsOfTypeAll<Entity>())
        {
            if (!entity.Any(e => e.name == go.name) && go.GetType() == typeof(Entity))
            entity.Add(go);
        }
        Debug.Log(Game_Data.Instance.aiCountForLevel1);
    }
    private void Start()
    {
        for (int i = 0; i < entity.Count; i++)
        {
            Game_Data.Instance.AddPlayer(entity[i].isAI, "Player" + "[" + i.ToString() + "]", entity[i].carBody.transform.localPosition);
            entity[i].SetPlayer(i);
            Game_Data.Instance.CheckList();
        }
    }
    
    public int GetPlayerIndex(Entity entity)
    {
        for (int i = 0; i < this.entity.Count; i++)
        {
            if (entity == this.entity[i]) return i;
        }
        return -1;
    }

    private void FixedUpdate()
    {
        globalTimer = Mathf.FloorToInt(Time.time);
        SycnTimer();
    }

    private void SycnTimer()
    {
        for (int i = 0; i < entity.Count;i++) 
        {
            Game_Data.Instance.GetIndexUpdateTimer(i, globalTimer);
        }
    }
}
