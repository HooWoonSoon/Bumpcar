using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrAi_State 
{
    protected PlayerOrAi_StateMachine stateMachine;
    protected Player_Controller player;
    protected Ai_Controller ai;

    private string animBoolName;

    public PlayerOrAi_State(Player_Controller _player, PlayerOrAi_StateMachine _stateMachine, string animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = animBoolName;
    }

    public PlayerOrAi_State(Ai_Controller _ai, PlayerOrAi_StateMachine _stateMachine, string animBoolName)
    {
        this.ai = _ai;
        this.stateMachine = _stateMachine;
        this.animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        if (player != null)
            player.animator.SetBool(animBoolName, true);
        else if (ai != null)
            ai.animator.SetBool(animBoolName, true);
    }
    public virtual void Update()
    {
        //Debug.Log("I'm in" + animBoolName);
    }

    public virtual void UpdateStateValue_Non_Ai(float horizontalInput, float verticalInput, float jumpInput) 
    {

    }

    public virtual void UpdateStateValue_Ai(float acceleration, float steer, float brake)
    {
    }

public virtual void Exit()
    {
        if (player != null)
            player.animator.SetBool(animBoolName, false);
        else if (ai != null)
            ai.animator.SetBool(animBoolName, false);
    }
}
