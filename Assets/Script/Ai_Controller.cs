using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum Personality
{
    Aggresive,
    Conservative,
    Balanced
}

[System.Serializable]
public class Ai_Controller : Entity
{
    public Circuit circuit;
    private DayNightCycle dayLight;
    public float steeringSensitivity = 0.01f;
    public float brakingSensitivity = 0.9f;
    public float accelerationSensitivity = 0.3f;
    private Vector3 target;
    private Vector3 nextTarget;
    private int currentWayPoint = 0;
    private int nextWayPoint;
    private int wayPointNumbers;
    private float totalDistanceToTarget;
    private bool isJump;

    [Header("Collect Item Range")]
    [SerializeField] private float detectItemRange;
    [SerializeField] private int randomChanceMin = 0; 
    [SerializeField] private int randomChanceMax = 5;
    private bool isTargetingItem;
    private Hashtable processedItem = new Hashtable();

    #region Ai Personality
    [SerializeField] private Personality personality;
    [SerializeField] private float personalityDuration = 8f;
    private float timerResetValue;
    #endregion

    private bool isReversing = false;

    private float lastTimeMoving = 0;

    private GameObject tracker;
    private int currentTackerWayPoint = 0;
    private float lookAhead = 10;

    public GameObject driveComponent;
    public GameObject walkComponent;

    #region Detection
    private float distanceOstacle;
    [SerializeField] private Transform[] raysTranform;
    [SerializeField] private float rayDistance = 9f;
    [SerializeField] private LayerMask wallMask;
    #endregion

    #region Drive internal
    public float steer { get; private set; }
    public float acceleration { get; private set; }
    public float brake { get; private set; }
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
        timerResetValue = personalityDuration;
    }
    private int SearchIndex()
    {
        Game_Manager manager = FindObjectOfType<Game_Manager>();
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
        RandomPeronality();
        DetectItem();

        if (drive._rigidbody.velocity.magnitude > 0.1f && switchMode == false || isFreeze)
            lastTimeMoving = Time.time;
        else if (drive._rigidbody.velocity.magnitude > 0.0001f && switchMode == true || isFreeze)
            lastTimeMoving = Time.time;

        //Debug.Log(drive._rigidbody.velocity.magnitude);

        if (Time.time > lastTimeMoving + 7)
        {
            ResearchTarget();
            carBody.transform.localPosition = gameData.characters[currentIndex].LastPosition;
            carBody.transform.localRotation = gameData.characters[currentIndex].LastRotation;
        }
        
        Vector3 localTarget;
        Vector3 nextLocalTarget = drive._rigidbody.gameObject.transform.InverseTransformPoint(nextTarget);
        float distanceTotarget = Vector3.Distance(target, drive._rigidbody.gameObject.transform.position);

        float targetAngle;
        float nextTargetAngle = Mathf.Atan2(nextLocalTarget.x, nextLocalTarget.z) * Mathf.Rad2Deg;

        if (Time.time < drive._rigidbody.GetComponent<Ai_AvoidDetection>().avoidTime)
            localTarget = tracker.transform.right * drive._rigidbody.GetComponent<Ai_AvoidDetection>().avoidPath;
        else
            localTarget = drive._rigidbody.gameObject.transform.InverseTransformPoint(target);

        targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float distanceFactor = distanceTotarget / totalDistanceToTarget;

        float corner = Mathf.Clamp(targetAngle, 0, 90f);
        float cornerFactor = corner / 90f;

        float speedFactor = drive.currentSpeed / drive.maxSpeed;

        if (isReversing)
        {
            acceleration = Mathf.Lerp(accelerationSensitivity * 1.2f, 1.2f, distanceFactor);
            brake = Mathf.Lerp(-1 - Mathf.Abs(nextTargetAngle), 1.5f + speedFactor, 1 - distanceFactor);
            steer = Mathf.Clamp(targetAngle * steeringSensitivity * 1.5f, -1, 1) * Mathf.Sign(drive.currentSpeed);
        }
        else
        {
            switch (personality)
            {
                case Personality.Aggresive:
                    acceleration = Mathf.Lerp(accelerationSensitivity * 1.2f, 1.2f, distanceFactor);
                    brake = Mathf.Lerp(-1 - Mathf.Abs(nextTargetAngle), 1.5f + speedFactor, 1 - distanceFactor);
                    if (!isTargetingItem)
                        steer = Mathf.Clamp(targetAngle * steeringSensitivity * 1.5f, -1, 1) * Mathf.Sign(drive.currentSpeed);
                    break;

                case Personality.Conservative:
                    acceleration = Mathf.Lerp(accelerationSensitivity * 0.7f, 0.7f, distanceFactor);
                    brake = Mathf.Lerp(-1 - Mathf.Abs(nextTargetAngle), 1.0f + speedFactor, 1 - distanceFactor);
                    if (!isTargetingItem)
                        steer = Mathf.Clamp(targetAngle * steeringSensitivity * 0.8f, -1, 1) * Mathf.Sign(drive.currentSpeed);
                    break;

                case Personality.Balanced:
                    acceleration = Mathf.Lerp(accelerationSensitivity, 1, distanceFactor);
                    brake = Mathf.Lerp(-1 - Mathf.Abs(nextTargetAngle), 1 + speedFactor, 1 - distanceFactor);
                    if (!isTargetingItem)
                        steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drive.currentSpeed);
                    break;
            }
        }

        if (corner > 10 && speedFactor > 0.1f)
            brake = Mathf.Lerp(0, 1 + speedFactor * brakingSensitivity, 1 - cornerFactor);

        if (corner > 20 && speedFactor > 0.2f)
            acceleration = Mathf.Lerp(0, 1 * accelerationSensitivity, 1 - cornerFactor);

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
        RaycastToObject();

        if (switchMode == false)
        {
            drive.Drive(acceleration, steer, brake);
            //Debug.Log(steer);
        }
        else
        {
            drive.AiHoldCarWalk(acceleration, steer);
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

    public void DetectItem()
    {
        Collider[] colliders = Physics.OverlapSphere(carBody.transform.position, detectItemRange);
        float closestDistance = Mathf.Infinity;
        float targetItemAngle;
        Vector3 closestItemPosition = Vector3.zero;

        foreach (var collider in colliders)
        {
            GameObject item = collider.gameObject;

            if (!processedItem.ContainsKey(item))
            {
                int randomChance = UnityEngine.Random.Range(randomChanceMin, randomChanceMax);
                processedItem.Add(item, randomChance);
            }

            if ((int)processedItem[item] != 0) continue;

            PowerUp powerUp = item.GetComponent<PowerUp>();
            PointStar pointStar = item.GetComponent<PointStar>();

            if (powerUp != null || pointStar != null)
            {
                float distanceToPowerUp = Vector3.Distance(item.transform.position, carBody.transform.position);
                if (distanceToPowerUp < closestDistance)
                {
                    closestDistance = distanceToPowerUp;
                    closestItemPosition = item.transform.position;
                }
            }

            if (closestDistance != Mathf.Infinity)
            {
                isTargetingItem = true;
                Vector3 localItemTarget = drive._rigidbody.gameObject.transform.InverseTransformPoint(closestItemPosition);
                targetItemAngle = Mathf.Atan2(localItemTarget.x, localItemTarget.z) * Mathf.Rad2Deg;
                steer = Mathf.Clamp(targetItemAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drive.currentSpeed);
            }
            else
                isTargetingItem = false;
        }
    }

    private void RaycastToObject()
    {
        for (int i = 0; i < raysTranform.Length; i++)
        {
            RaycastHit[] raycastHits = Physics.RaycastAll(raysTranform[i].transform.position, raysTranform[i].forward, rayDistance, wallMask);
            foreach (RaycastHit obstacle in raycastHits)
            {
                distanceOstacle = Vector3.Distance(raysTranform[i].transform.position, obstacle.transform.position);
                //Debug.Log(obstacle.collider.name + ":" + distanceOstacle);

                if (distanceOstacle <= 8f && acceleration >= 0)
                {
                    brake = Mathf.Lerp(0, 1 + drive.currentSpeed / drive.maxSpeed * brakingSensitivity, 1 - rayDistance / distanceOstacle);
                }

                if (distanceOstacle <= 5.5f)
                {
                    if (drive.currentSpeed > 0)
                    {
                        isReversing = true;
                        acceleration = -acceleration* 0.5f;
                        steer = -steer;
                        return;
                    }
                }
            }
        }

        if (isReversing && drive.currentSpeed <= 0)
        {
            isReversing = false;
            acceleration = 0f; // zero the acceleration when back car
        }
    }

    public void RandomPeronality()
    {
        if (personalityDuration > 0)
            personalityDuration -= Time.deltaTime;
        else
        {
            Array values = Enum.GetValues(typeof(Personality));
            personality = (Personality)values.GetValue(UnityEngine.Random.Range(0, values.Length));

            personalityDuration = timerResetValue;
        }
    }

    //public void ResearchTarget()
    //{
    //    List<(float, int, Vector3)> TotaldistanceToTarget = new List<(float, int, Vector3)>();
    //    float distanceToTargetRespawn;

    //    for (int i = 0; i < circuit.waypoints.Length; i++)
    //    {
    //        distanceToTargetRespawn = Vector3.Distance(circuit.waypoints[i].transform.position, drive._rigidbody.gameObject.transform.position);
    //        TotaldistanceToTarget.Add((distanceToTargetRespawn, i, circuit.waypoints[i].transform.position));
    //    }

    //    TotaldistanceToTarget = TotaldistanceToTarget.OrderBy(d => d.Item1).ToList();
    //    currentTackerWayPoint = TotaldistanceToTarget.First().Item2;
    //    currentWayPoint = TotaldistanceToTarget.First().Item2;
    //    target = circuit.waypoints[currentWayPoint].transform.position;

    //    if (currentWayPoint <= wayPointNumbers)
    //    {
    //        nextWayPoint = Mathf.Clamp(currentWayPoint + 1, 0, wayPointNumbers);
    //        nextTarget = circuit.waypoints[nextWayPoint].transform.position;
    //    }

    //    tracker.transform.position = circuit.waypoints[currentTackerWayPoint].transform.position;
    //    Debug.Log(TotaldistanceToTarget.First().Item1 + "," + TotaldistanceToTarget.First().Item2 + "," + TotaldistanceToTarget.First().Item3);
    //}

    public void ResearchTarget()
    {
        currentWayPoint = gameData.characters[currentIndex].WayPoint;

        if (currentIndex != -1)
        {
            if (currentWayPoint <= wayPointNumbers)
            {
                nextWayPoint = Mathf.Clamp(currentWayPoint + 1, 0, wayPointNumbers);
                nextTarget = circuit.waypoints[nextWayPoint].transform.position;
                currentTackerWayPoint = currentWayPoint;
                target = circuit.waypoints[currentWayPoint].transform.position;
            }
        }
        else
        {
            float closestTarget = Mathf.Infinity;
            float distanceToTargetRespawn;
            int closestWaypointIndex = -1;

            for (int i = 0; i < circuit.waypoints.Length; i++)
            {
                distanceToTargetRespawn = Vector3.Distance(circuit.waypoints[i].transform.position, drive._rigidbody.gameObject.transform.position);
                if (closestTarget > distanceToTargetRespawn)
                {
                    closestTarget = distanceToTargetRespawn;
                    closestWaypointIndex = i;
                }
            }

            if (closestWaypointIndex != -1)
            {
                currentWayPoint = closestWaypointIndex;
                currentTackerWayPoint = closestWaypointIndex;
                target = circuit.waypoints[currentWayPoint].transform.position;
            }

            if (currentWayPoint <= wayPointNumbers)
            {
                nextWayPoint = Mathf.Clamp(currentWayPoint + 1, 0, wayPointNumbers);
                nextTarget = circuit.waypoints[nextWayPoint].transform.position;
            }
        }

        tracker.transform.position = circuit.waypoints[currentTackerWayPoint].transform.position;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < raysTranform.Length; i++)
        {
            Gizmos.DrawLine(raysTranform[i].position, raysTranform[i].position + raysTranform[i].forward * rayDistance);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(carBody.transform.position, detectItemRange);
    }
}