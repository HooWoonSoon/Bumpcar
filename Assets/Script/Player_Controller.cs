using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private Wheel_Drive drive;
    public Animator animator { get; private set; }
    private bool LampActive = false;
    private bool SwitchMode = false;

    #region State
    public Player_StateMachine stateMachine {  get; private set; } 
    public Player_DriveIdleState driveIdleState { get; private set; }
    public Player_DriveState driveState { get; private set; }
    public Player_HoldCarIdle holdCarIdle { get; private set; }
    public Player_HoldCarMove holdCarMove { get; private set; }
    #endregion

    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float JumpOrBreakInput;

    private void Awake()
    {
        stateMachine = new Player_StateMachine();
        driveIdleState = new Player_DriveIdleState(this, stateMachine, "DriveIdle");
        driveState = new Player_DriveState(this, stateMachine, "Drive");
        holdCarIdle = new Player_HoldCarIdle(this, stateMachine, "HoldCarIdle");
        holdCarMove = new Player_HoldCarMove(this, stateMachine, "HoldCarMove");
    }


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        stateMachine.Initialize(driveIdleState);

        this.drive = GetComponent<Wheel_Drive>();
        drive.SetLampActivate(LampActive);
    }

    void Update()
    {
        stateMachine.currentState.Update();
        horizontalInput = Input.GetAxis("Vertical");
        verticalInput = Input.GetAxis("Horizontal");
        JumpOrBreakInput = Input.GetAxis("Jump");

        drive.Drive(horizontalInput, verticalInput, JumpOrBreakInput);
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
        }
    }
}
