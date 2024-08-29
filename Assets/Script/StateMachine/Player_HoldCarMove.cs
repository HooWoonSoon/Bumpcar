using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player_HoldCarMove : Player_State
{
    public Player_HoldCarMove(Player_Controller _player, Player_StateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
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

        if (horizontal == 0 && vertical == 0)
        {
            stateMachine.ChangState(player.holdCarIdle);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            stateMachine.ChangState(player.driveIdleState);
        }
    }
}
