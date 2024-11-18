using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;
    public float heightAbovePlayer = 10f;

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPosition = player.position;
            newPosition.y += heightAbovePlayer; 
            transform.position = newPosition;

        }
    }
}
