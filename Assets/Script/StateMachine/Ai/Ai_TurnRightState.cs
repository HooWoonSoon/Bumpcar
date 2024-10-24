using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Ai_TurnRightState : PlayerOrAi_State
{
    public Ai_TurnRightState(Ai_Controller _ai, PlayerOrAi_StateMachine _stateMachine, string animBoolName) : base(_ai, _stateMachine, animBoolName)
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

        if (steer < 0.003)
        {
            stateMachine.ChangState(ai.moveState);
        }
    }
}
