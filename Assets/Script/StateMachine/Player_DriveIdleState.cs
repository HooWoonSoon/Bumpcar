using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DriveIdleState : Player_State
{
    public Player_DriveIdleState(Player_Controller _player, Player_StateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
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

    public override void UpdateStateValue(float horizontal, float vertical, float jump )
    {
        base.UpdateStateValue(horizontal, vertical, jump);


        if (horizontal != 0 || vertical != 0)
        {
            stateMachine.ChangState(player.driveState);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            stateMachine.ChangState(player.holdCarIdle);
        }
    }
}
