using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Entity entity;

    private static HashSet<int> playerCheckpoints = new HashSet<int>();

    public int checkpointIndex; 

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
                if (!playerCheckpoints.Contains(checkpointIndex))
                {
                    playerCheckpoints.Add(checkpointIndex);
                    Debug.Log($"Player passed checkpoint {checkpointIndex}");
                }
            }

            if (ai != null)
            {
                entity.UpdatePosition(ai.currentIndex, ai.carBody.transform.localPosition);
            }
        }
    }

    public static bool PlayerClearedAllCheckpoints(int totalCheckpoints)
    {
        Debug.Log($"Player cleared checkpoints: {playerCheckpoints.Count} / {totalCheckpoints}");
        return playerCheckpoints.Count >= totalCheckpoints;
    }
}
