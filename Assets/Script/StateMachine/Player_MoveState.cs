using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MoveState : Player_State
{
    public Player_MoveState(Player_Controller _player, Player_StateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
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

        if (horizontal == 0 && vertical == 0 || Input.GetKeyDown(KeyCode.LeftControl))
        {
            stateMachine.ChangState(player.IdleState);
        }
        else if (horizontal < 0)
        {
            stateMachine.ChangState(player.turnLeftState);
        }
        else if (horizontal > 0)
        {
            stateMachine.ChangState(player.turnRightState);
        }
    }
}
