using UnityEngine;

public class PointStar : MonoBehaviour
{
    private Game_Manager manager;
    private Entity entity;
    private Ai_Controller ai; // temporarily

    private void Start()
    {
        entity = FindAnyObjectByType<Entity>();
    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.tag == "Car")
        {
            entity.AddPoint();
            this.gameObject.SetActive(false);
            Debug.Log("IsAddPoint");
        }
    }
}