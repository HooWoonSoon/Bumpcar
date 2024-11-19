using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_DetectedRockPlane : MonoBehaviour
{
    private Ai_Controller ai;

    private void Start()
    {
        ai = GetComponentInParent<Ai_Controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RockPlane"))
        {
            ai.switchMode = true;
            ai.animator = ai.walkComponent.GetComponent<Animator>();
            ai.stateMachine.Initialize(ai.idleState);
            ai.drive.SwitchModeCollider(ai.switchMode);
            ai.driveComponent.SetActive(!ai.switchMode);
            ai.walkComponent.SetActive(ai.switchMode);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RockPlane"))
        {
            ai.switchMode = false;
            ai.animator = ai.driveComponent.GetComponent<Animator>();
            ai.stateMachine.Initialize(ai.idleState);
            ai.drive.SwitchModeCollider(ai.switchMode);
            ai.driveComponent.SetActive(!ai.switchMode);
            ai.walkComponent.SetActive(ai.switchMode);
        }
    }
}