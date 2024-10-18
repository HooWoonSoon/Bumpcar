using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_TurnRightState : Player_State
{
    public Player_TurnRightState(Player_Controller _player, Player_StateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
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

    public override void UpdateStateValue(float horizontal, float vertical, float jump)
    {
        base.UpdateStateValue(horizontal, vertical, jump);

        if (horizontal <= 0)
        {
            stateMachine.ChangState(player.moveState);
        }
    }
}
