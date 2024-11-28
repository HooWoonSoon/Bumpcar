using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Hud : MonoBehaviour
{
    private Game_Data gameData;
    private Game_Manager gameManager;
    private Player_Controller player;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI spawnTimeText;
    [SerializeField] private TextMeshProUGUI starPoint;
    [SerializeField] private Image speedBooster;

    private float elapsedTime;
    
    void Start()
    {
        gameManager = FindObjectOfType<Game_Manager>();
        gameData = Game_Data.Instance;
        player = FindObjectOfType<Player_Controller>();
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

        float alpha = 1f;

        if (player.timer <= 2f && player.timer > 0)
            alpha = Mathf.PingPong(Time.time * 5f, 1f);  // Flashing effect
        else if (player.timer <= 0)
            alpha = 0f;

        SetSpeedBoosterAlpha(alpha); // Update the icon's alpha transparency
    }

    private void SetSpeedBoosterAlpha(float alpha)
    {
        Color currentColor = speedBooster.color;
        currentColor.a = alpha;
        speedBooster.color = currentColor;
    }
}
