using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockOut : MonoBehaviour
{
    private Entity entity;
    private Game_Data gameData;

    private void Start()
    {
        entity = FindAnyObjectByType<Entity>();
        gameData = Game_Data.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DetectBox"))
        {
            Player_Controller player = other.GetComponentInParent<Player_Controller>();
            Ai_Controller ai = other.GetComponentInParent<Ai_Controller>();
            Wheel_Drive drive = other.GetComponentInParent<Wheel_Drive>();
            if (player != null && drive != null)
            {
                entity.AddDead(player.currentIndex);
                player.carBody.transform.localPosition = gameData.characters[player.currentIndex].LastCheckpoint;
                player.carBody.transform.localRotation = Quaternion.Euler(0, player.carBody.transform.localRotation.eulerAngles.y, 0);
                drive._rigidbody.velocity = Vector3.zero;
                drive._rigidbody.angularVelocity = Vector3.zero;
            }
            if (ai != null && drive != null)
            {
                entity.AddDead(ai.currentIndex);
                ai.ResearchTarget();
                ai.carBody.transform.localPosition = gameData.characters[ai.currentIndex].LastCheckpoint;
                ai.carBody.transform.localRotation = Quaternion.Euler(0, ai.carBody.transform.localRotation.eulerAngles.y, 0);
                drive._rigidbody.velocity = Vector3.zero;
                drive._rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
}
