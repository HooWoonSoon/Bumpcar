using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Ai_MoveState : PlayerOrAi_State
{
    public Ai_MoveState(Ai_Controller _ai, PlayerOrAi_StateMachine _stateMachine, string animBoolName) : base(_ai, _stateMachine, animBoolName)
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
        Debug.Log("Acceleration" + acceleration + "Steer" + steer);
        if (ai != null && stateMachine != null)
        {
            if (acceleration == 0 && steer == 0) //|| Input.GetKeyDown(KeyCode.LeftControl)) 
            {
                stateMachine.ChangState(ai.idleState);
            }
            else if (acceleration < 0)
            {
                stateMachine.ChangState(ai.turnLeftState);
            }
            else if (acceleration > 0)
            {
                stateMachine.ChangState(ai.turnRightState);
            }

        }
    }
}
