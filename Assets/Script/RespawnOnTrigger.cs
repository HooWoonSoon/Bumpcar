using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOnTrigger : MonoBehaviour
{
    // This will store the previous position of the character
    private Vector3 previousPosition;

    void Start()
    {
        // Save the starting position as the initial respawn point
        previousPosition = transform.position;
    }

    // This function is called when the character enters a trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object the character touched has the "RespawnTrigger" tag
        if (other.gameObject.CompareTag("RespawnTrigger"))
        {
            // Respawn the character at the previous position
            transform.position = previousPosition;
        }
        // Check if the object the character touched has the "Checkpoint" tag
        else if (other.gameObject.CompareTag("Checkpoint"))
        {
            // Update the respawn location to checkpoint's position
            previousPosition = transform.position;
        }
    }

    public void UpdateRespawnLocation(Vector3 newPosition)
    {
        previousPosition = newPosition;
    }

}
