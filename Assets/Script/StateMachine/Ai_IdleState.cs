using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_IdleState : PlayerOrAi_State
{
    public Ai_IdleState(Ai_Controller _ai, PlayerOrAi_StateMachine _stateMachine, string animBoolName) : base(_ai, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateStateValue_Ai(float acceleration, float steer, float brake)
    {
        base.UpdateStateValue_Ai(acceleration, steer, brake);

        if (acceleration > 0 || steer > 0)
        {
            stateMachine.ChangState(ai.moveState);
        }
    }
}
