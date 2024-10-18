using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Ai_Controller : Entity
{
    private Entity entity;
    public Circuit circuit;
    Wheel_Drive drive;
    public float steeringSensitivity = 0.01f;
    public float accelerationSensitivity = 0.3f;
    private Vector3 target;
    private Vector3 nextTarget;
    private int currentWayPoint = 0;
    private int nextWayPoint;
    private int wayPointNumbers;
    private float totalDistanceToTarget;
    private bool isJump;

    private GameObject tracker;
    private int currentTackerWayPoint = 0;
    private float lookAhead = 10;



    protected override void Awake()
    {
        base.Awake();
        entity = GetComponent<Entity>();
        
    }
    protected override void Start()
    {
        base.Start();
        drive = GetComponent<Wheel_Drive>();
        target = circuit.waypoints[currentWayPoint].transform.position;
        nextTarget = circuit.waypoints[currentWayPoint + 1].transform.position;
        totalDistanceToTarget = Vector3.Distance(target, drive._rigidbody.gameObject.transform.position);
        wayPointNumbers = circuit.waypoints.Length - 1;

        tracker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.transform.position = drive._rigidbody.gameObject.transform.position;
        transform.transform.rotation = drive._rigidbody.gameObject.transform.rotation;
        currentIndex = SearchIndex();
    }
    private int SearchIndex()
    {
        Game_Manager manager = FindAnyObjectByType<Game_Manager>();
        return manager.GetPlayerIndex(entity); // don't touch please
    }

    private void ProgressTracker()
    {
        Debug.DrawLine(drive._rigidbody.gameObject.transform.position, tracker.transform.position);

        if (Vector3.Distance(drive._rigidbody.gameObject.transform.position, tracker.transform.position) > lookAhead) { return; }

        tracker.transform.LookAt(circuit.waypoints[currentTackerWayPoint].transform.position);
        tracker.transform.Translate(0, 0, 1f);
        
        if (Vector3.Distance(tracker.transform.position, circuit.waypoints[currentTackerWayPoint].transform.position) < 1f)
        {
            currentTackerWayPoint++;
            if (currentTackerWayPoint >= circuit.waypoints.Length)
            {
                currentTackerWayPoint = 0;
            }
        }
    }

    void Update()
    {
        ProgressTracker();
        Vector3 localTarget = drive._rigidbody.gameObject.transform.InverseTransformPoint(target);
        Vector3 nextLocalTarget = drive._rigidbody.gameObject.transform.InverseTransformPoint(nextTarget);
        float distanceTotarget = Vector3.Distance(target,drive._rigidbody.gameObject.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        float nextTargetAngle = Mathf.Atan2(nextLocalTarget.x, nextLocalTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drive.currentSpeed);

        float distanceFactor = distanceTotarget / totalDistanceToTarget;
        
        float speedFactor = drive.currentSpeed / drive.maxSpeed;

        float acceleration = Mathf.Lerp(accelerationSensitivity, 1, distanceFactor);
        float brake = Mathf.Lerp(-1 -Mathf.Abs(nextTargetAngle) , 1 + speedFactor, 1 - distanceFactor);

        if (Mathf.Abs(nextTargetAngle) > 20)
        {
            brake += 0.8f;
            acceleration -= 0.6f;
        }

        if (isJump)
        {
            acceleration = 1;
            brake = 0;
            Debug.Log(isJump);
        }

        //Debug.Log("Brake: " +  brake + "Steer:" + steer + "Speed:" + drive._rigidbody.velocity.magnitude + "Acceleration:" + acceleration);

        drive.Drive(acceleration,steer,brake);


        if (distanceTotarget < 2) // threshold, make larger if car start to circle waypoint
        {
            currentWayPoint++;
            if (currentWayPoint >= circuit.waypoints.Length)
            {
                currentWayPoint = 0;
            }
            totalDistanceToTarget = Vector3.Distance(target, drive._rigidbody.gameObject.transform.position);
            target = circuit.waypoints[currentWayPoint].transform.position;
            if (currentWayPoint <= wayPointNumbers)
            {
                nextWayPoint = Mathf.Clamp(currentWayPoint + 1, 0, wayPointNumbers);
                nextTarget = circuit.waypoints[nextWayPoint].transform.position;
            }
            
            if (drive._rigidbody.gameObject.transform.InverseTransformPoint(target).y > 5)
            {
                isJump = true;
            }
            else
            {
                isJump = false;
            }

        }
    }
}
