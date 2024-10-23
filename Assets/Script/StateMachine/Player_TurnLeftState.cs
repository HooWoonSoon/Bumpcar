using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_TurnLeftState : PlayerOrAi_State
{
    public Player_TurnLeftState(Player_Controller _player, PlayerOrAi_StateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
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

    public override void UpdateStateValue_Non_Ai(float horizontal, float vertical, float jump)
    {
        base.UpdateStateValue_Non_Ai(horizontal, vertical, jump);

        if (horizontal >= 0)
        {
            stateMachine.ChangState(player.moveState);
        }
    }
}
