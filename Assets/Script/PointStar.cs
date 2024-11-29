using UnityEngine;

public class PointStar : MonoBehaviour
{
    private Entity entity;

    private void Start()
    {
        entity = FindAnyObjectByType<Entity>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DetectBox") return;

        Player_Controller player = other.GetComponentInParent<Player_Controller>();
        Ai_Controller ai = other.GetComponentInParent<Ai_Controller>();
        if (player != null)
        {
            entity.AddPoint(player.currentIndex);
            gameObject.SetActive(false);
        }
        if (ai != null)
        {
            entity.AddPoint(ai.currentIndex);
            gameObject.SetActive(false);
        }
    }
}