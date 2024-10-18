using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : Entity
{
    private Wheel_Drive drive;
    private Entity entity;
    private Game_Manager manager;
    public Animator animator { get; private set; }
    private bool LampActive = false;
    private bool SwitchMode = false;

    #region State
    public Player_StateMachine stateMachine {  get; private set; } 
    public Player_idleState IdleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_TurnLeftState turnLeftState { get; private set; }
    public Player_TurnRightState turnRightState { get; private set; }
    #endregion

    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float JumpOrBreakInput;

    [SerializeField] private GameObject driveComponent;
    [SerializeField] private GameObject walkComponent;
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new Player_StateMachine();
        entity = GetComponent<Entity>();
        IdleState = new Player_idleState(this, stateMachine, "Idle");
        moveState = new Player_MoveState(this, stateMachine, "Move");
        turnLeftState = new Player_TurnLeftState(this, stateMachine, "TurnLeft");
        turnRightState = new Player_TurnRightState(this, stateMachine, "TurnRight");
    }

    protected override void Start()
    {
        base.Start();
        manager = FindAnyObjectByType<Game_Manager>();
        driveComponent.SetActive(!SwitchMode);
        walkComponent.SetActive(SwitchMode);
        animator = GetComponentInChildren<Animator>();
        stateMachine.Initialize(IdleState);;
        this.drive = GetComponent<Wheel_Drive>();
        drive.SetLampActivate(LampActive);
        currentIndex = SearchIndex();
    }
    private int SearchIndex()
    {
        return manager.GetPlayerIndex(entity); // don't touch please
    }

    void Update()
    {
        stateMachine.currentState.Update();
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        JumpOrBreakInput = Input.GetAxis("Jump");

        if (SwitchMode == false)
        {
            drive.Drive(verticalInput, horizontalInput, JumpOrBreakInput);
        }
        else
        {
            drive.HoldCarWalk(verticalInput, horizontalInput, JumpOrBreakInput);   
        }
        // Inheritance the UpdateState is not similar like Monobevoiur "Update" it just a name, it used to reduced redundancy @Woon Soon ^_^
        stateMachine.currentState.UpdateStateValue(horizontalInput, verticalInput, JumpOrBreakInput); 

        if (Input.GetKeyDown(KeyCode.Q))
        {
            LampActive = !LampActive;
            drive.SetLampActivate(LampActive);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SwitchMode = !SwitchMode;
            drive.SwitchModeCollider(SwitchMode);
            driveComponent.SetActive(!SwitchMode);
            walkComponent.SetActive(SwitchMode);
            animator = GetComponentInChildren<Animator>();
        }
    }
}
