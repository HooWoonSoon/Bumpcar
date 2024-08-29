using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State 
{
    protected Player_StateMachine stateMachine;
    protected Player_Controller player;

    private string animBoolName;

    public Player_State(Player_Controller _player, Player_StateMachine _stateMachine, string animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.animator.SetBool(animBoolName, true);
    }
    public virtual void Update()
    {
        Debug.Log("I'm in" + animBoolName);
    }

    public virtual void UpdateStateValue(float horizontalInput, float verticalInput, float jumpInput) 
    {
        Debug.Log("Using Move Module");
    }

public virtual void Exit()
    {
        player.animator.SetBool(animBoolName, false);
    }
}
