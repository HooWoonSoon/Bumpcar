using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player_Controller player = other.GetComponentInParent<Player_Controller>();
        Ai_Controller ai = other.GetComponentInParent<Ai_Controller>();
        if (player != null)
        {
            player.StartCoroutine(player.PowerUpSpeed(2, 6));
            gameObject.SetActive(false);
        }
        if (ai != null)
        {
            ai.StartCoroutine(ai.PowerUpSpeed(2, 6));
            gameObject.SetActive(false);
        }
    }
}
