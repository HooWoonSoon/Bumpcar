using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public List<Entity> player;

    #region Global timer
    [SerializeField] private float variationTime = 150f;
    public float globalTimer;
    private float nextToggleTime;
    public bool daylight;
    #endregion
    private void Start()
    {
        for (int i = 0; i < player.Count; i++)
        {
            Game_Data.Instance.AddPlayer(player[i].isAI, "Player" + "[" + i.ToString() + "]");
            player[i].SetPlayer(i);
        }
        daylight = true;
    }
    
    public int GetPlayerIndex(Entity entity)
    {
        for (int i = 0; i < player.Count; i++)
        {
            if (entity == player[i]) return i;
        }
        return -1;
    }

    private void FixedUpdate()
    {
        globalTimer = Mathf.FloorToInt(Time.time);
        if (globalTimer >= nextToggleTime)
        {
            daylight = !daylight;
            Debug.Log(daylight);

            nextToggleTime += variationTime;
        }
        SycnTimer();
    }

    private void SycnTimer()
    {
        for (int i = 0; i < player.Count;i++) 
        {
            Game_Data.Instance.GetIndexUpdateTimer(i, globalTimer);
        }
    }
}
