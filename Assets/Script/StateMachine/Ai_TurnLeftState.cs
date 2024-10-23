using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Ai_TurnLeftState : PlayerOrAi_State
{
    public Ai_TurnLeftState(Ai_Controller _ai, PlayerOrAi_StateMachine _stateMachine, string animBoolName) : base(_ai, _stateMachine, animBoolName)
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

        if (acceleration >= 0)
        {
            stateMachine.ChangState(ai.moveState);
        }
        else if (steer > 0)
        {
            stateMachine.ChangState(ai.turnRightState);
        }
    }
}
