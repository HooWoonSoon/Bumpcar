using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Ai_Controller : Entity
{
    public Circuit circuit;
    public Wheel_Drive drive {  get; private set; }
    private DayNightCycle dayLight;
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

    public GameObject driveComponent;
    public GameObject walkComponent;

    #region Drive internal
    public float steer {  get; private set; }
    public float acceleration { get; private set; }
    public float brake {  get; private set; }
    #endregion

    [HideInInspector] public bool switchMode = false;
    private Entity entity;
    public PlayerOrAi_StateMachine stateMachine { get; private set; }
    public Ai_IdleState idleState { get; private set; }
    public Ai_MoveState moveState { get; private set; }
    public Ai_TurnLeftState turnLeftState { get; private set; }
    public Ai_TurnRightState turnRightState { get; private set; }
    public Animator animator;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerOrAi_StateMachine();
        idleState = new Ai_IdleState(this, stateMachine, "Idle");
        moveState = new Ai_MoveState(this, stateMachine, "Move");
        turnLeftState = new Ai_TurnLeftState(this, stateMachine, "TurnLeft");
        turnRightState = new Ai_TurnRightState(this, stateMachine, "TurnRight");
        entity = GetComponent<Entity>();
    }
    protected override void Start()
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
        stateMachine.Initialize(idleState);
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
        dayLight = FindAnyObjectByType<DayNightCycle>();
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
    private void IsCarLight() => drive.SetLampActivate(!dayLight.daylight);

    void Update()
    {
        ProgressTracker();
        IsCarLight();
        //RockPlaneDetected();
        Vector3 localTarget = drive._rigidbody.gameObject.transform.InverseTransformPoint(target);
        Vector3 nextLocalTarget = drive._rigidbody.gameObject.transform.InverseTransformPoint(nextTarget);
        float distanceTotarget = Vector3.Distance(target,drive._rigidbody.gameObject.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        float nextTargetAngle = Mathf.Atan2(nextLocalTarget.x, nextLocalTarget.z) * Mathf.Rad2Deg;

        steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drive.currentSpeed);

        float distanceFactor = distanceTotarget / totalDistanceToTarget;
        
        float speedFactor = drive.currentSpeed / drive.maxSpeed;

        acceleration = Mathf.Lerp(accelerationSensitivity, 1, distanceFactor);
        brake = Mathf.Lerp(-1 -Mathf.Abs(nextTargetAngle) , 1 + speedFactor, 1 - distanceFactor);
        if (Mathf.Abs(nextTargetAngle) > 20)
        {
            brake += 0.8f;
            acceleration -= 0.6f;
        }

        if (isJump)
        {
            acceleration = 1;
            brake = 0;
        }

        //Debug.Log("Brake: " +  brake + "Steer:" + steer + "Speed:" + drive._rigidbody.velocity.magnitude + "Acceleration:" + acceleration);

        if (switchMode == false)
        {
            drive.Drive(acceleration,steer,brake);
            //Debug.Log(steer);
        }
        else
        {
            drive.AiHoldCarWalk(acceleration,steer);
        }
        stateMachine.currentState.UpdateStateValue_Ai(acceleration, steer, brake);

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
