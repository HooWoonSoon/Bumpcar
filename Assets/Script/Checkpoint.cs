using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Entity entity;

    private void Start()
    {
        entity = FindAnyObjectByType<Entity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DetectBox"))
        {
            Player_Controller player = other.GetComponentInParent<Player_Controller>();
            Ai_Controller ai = other.GetComponentInParent<Ai_Controller>();
            if (player != null)
            {
                entity.UpdatePosition(player.currentIndex, player.carBody.transform.localPosition);
            }
            if (ai != null)
            {
                entity.UpdatePosition(ai.currentIndex, ai.carBody.transform.localPosition);
            }
        }
    }
}
