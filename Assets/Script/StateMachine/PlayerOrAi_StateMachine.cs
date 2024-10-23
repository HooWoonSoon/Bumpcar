using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrAi_StateMachine
{
    public PlayerOrAi_State currentState {  get; private set; }
    
    public void Initialize(PlayerOrAi_State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangState(PlayerOrAi_State newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter(); 
    }
}
