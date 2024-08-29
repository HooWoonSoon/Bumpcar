using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltAi_Controller : MonoBehaviour
{
    [SerializeField] private Circuit circuit;
    private Vector3 target;
    private int currentWayPoint;
    private float speed = 20f;
    private float accuracy = 4.0f;
    private float rotateSpeed = 5.0f;
    void Start()
    {
        target = circuit.waypoints[currentWayPoint].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceTarget = Vector3.Distance(target, this.transform.position);
        Vector3 direction = target - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotateSpeed);

        this.transform.Translate(0,0,speed * Time.deltaTime);

        if (distanceTarget < accuracy)
        {
            currentWayPoint++;
            if (currentWayPoint >= circuit.waypoints.Length)
            {
                currentWayPoint = 0;
            }
            target = circuit.waypoints[currentWayPoint].transform.position;
        }
    }
}
