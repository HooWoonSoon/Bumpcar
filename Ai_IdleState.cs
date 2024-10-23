using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_IdleState : PlayerOrAi_State
{
    public Ai_IdleState(Ai_Controller _ai, PlayerOrAi_StateMachine _stateMachine, string animBoolName) : base(_ai, _stateMachine, animBoolName)
    {
    }
}
