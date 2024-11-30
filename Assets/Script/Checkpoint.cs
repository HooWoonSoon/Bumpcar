using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Entity entity;

    private static HashSet<int> playerCheckpoints = new HashSet<int>();

    public int checkpointIndex;
    [SerializeField] private GameObject wayPoint;
    [SerializeField] private Circuit circuit;
    private int waypointIndex;

    private void Start()
    {
        entity = FindAnyObjectByType<Entity>();
        waypointIndex = -1;
        
        for (int i = 0; i < circuit.waypoints.Length; i++)
        {
            if (circuit.waypoints[i].gameObject == wayPoint)
            {
                waypointIndex = i;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DetectBox"))
        {
            Player_Controller player = other.GetComponentInParent<Player_Controller>();
            Ai_Controller ai = other.GetComponentInParent<Ai_Controller>();

            if (player != null)
            {
                entity.UpdatePosition(player.currentIndex, player.carBody.transform.localPosition, waypointIndex, player.carBody.transform.localRotation);
                if (!playerCheckpoints.Contains(checkpointIndex))
                {
                    playerCheckpoints.Add(checkpointIndex);
                    Debug.Log($"Player passed checkpoint {checkpointIndex}");
                }
            }

            if (ai != null)
            {
                entity.UpdatePosition(ai.currentIndex, ai.carBody.transform.localPosition, waypointIndex, ai.carBody.transform.localRotation);
            }
        }
    }

    public static bool PlayerClearedAllCheckpoints(int totalCheckpoints)
    {
        Debug.Log($"Player cleared checkpoints: {playerCheckpoints.Count} / {totalCheckpoints}");
        return playerCheckpoints.Count >= totalCheckpoints;
    }
}
