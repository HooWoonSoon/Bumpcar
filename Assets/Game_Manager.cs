using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public Entity player1;
    public Entity player2;

    private void Start()
    {
        Game_Data.Instance.AddPlayer(false); 
        Game_Data.Instance.AddPlayer(false); 

        player1.SetPlayer(0); 
        player2.SetPlayer(1);  
    }
}
