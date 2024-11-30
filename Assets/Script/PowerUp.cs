using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerType
{
    None,
    SpeedUp,
    Freeze
}

public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerType powerType;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DetectBox") return;

        Player_Controller player = other.GetComponentInParent<Player_Controller>();
        Ai_Controller ai = other.GetComponentInParent<Ai_Controller>();
        if (player != null)
        {
            player.StartCoroutine(player.PowerUpSpeed(1.5f, 4, powerType));
            gameObject.SetActive(false);
        }
        if (ai != null)
        {
            ai.StartCoroutine(ai.PowerUpSpeed(2, 4, powerType));
            gameObject.SetActive(false);
        }
    }
}
