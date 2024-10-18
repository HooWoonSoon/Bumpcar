using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player_Hud : MonoBehaviour
{
    private Game_Data gameData;
    private Game_Manager gameManager;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI spawnTimeText;
    [SerializeField] private TextMeshProUGUI starPoint;
    
    private float elapsedTime;
    
    void Start()
    {
        gameManager = FindAnyObjectByType<Game_Manager>();
        gameData = Game_Data.Instance;
    }
    
    void Update()
    {
        elapsedTime = gameManager.globalTimer;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}",minutes,seconds);

        foreach (var player in gameData.characters)
        {
            if (player.IsAi == false)
            {
                starPoint.text = player.StarCount.ToString();
                spawnTimeText.text = player.PlayerSpawnCount.ToString();
            }
        }
    }
}
