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
    [SerializeField] private Image freeze;

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

        SetIconAlpha(speedBooster, 0f);
        SetIconAlpha(freeze, 0f);
        if (player.activateType == PowerType.SpeedUp)
        {
            SetIconAlpha(speedBooster, CalculateAlpha(player.timer));
        }
        else if (player.activateType == PowerType.Freeze)
        {
            SetIconAlpha(freeze, CalculateAlpha(player.timer));
        }
    }

    private float CalculateAlpha(float timer)
    {
        if (timer <= 3f && timer > 0)
            return Mathf.PingPong(Time.time * 5f, 1f); // Flashing effect
        else if (timer <= 0)
            return 0f; // Fully transparent
        return 1f; // Fully visible
    }

    private void SetIconAlpha(Image icon, float alpha)
    {
        if (icon == null) return;

        Color currentColor = icon.color;
        currentColor.a = alpha;
        icon.color = currentColor;
    }
}
