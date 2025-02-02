﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Wheel_Drive : MonoBehaviour
{
    [Header("Drive")]
    [SerializeField] private WheelCollider[] _wheelCollider;
    public float torque = 80f;
    [SerializeField] private float maxStreerAngle = 30f;
    [SerializeField] private float maxBrakeTorque = 500f;
    [SerializeField] private GameObject[] Wheels;
    [SerializeField] private GameObject[] Lamp;
    [SerializeField] private GameObject brakeLight;
    public float maxSpeed = 150f;
    private float changeMaxSpeed;

    public Rigidbody _rigidbody;

    [Header("Walk")]
    [SerializeField] private Transform HoldCarTransform;
    [SerializeField] private BoxCollider carCollider;
    [SerializeField] BoxCollider characterCollider;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float jumpForce = 10f;
    private float coolDownTimer;

    [SerializeField] private Transform boxGroundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] LayerMask ground;

    public float currentSpeed { get { return _rigidbody.velocity.magnitude; } }

    private void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }
    void Start()
    {
        _rigidbody.centerOfMass = new Vector3(_rigidbody.centerOfMass.x, -0.5f, _rigidbody.centerOfMass.z);
        characterCollider.enabled = false;

        brakeLight.SetActive(false);
    }

    public void AiHoldCarWalk(float accelerations, float steer)
    {
        accelerations = Mathf.Clamp(Mathf.Abs(accelerations), 0.3f, 0.7f);
        steer = Mathf.Clamp(steer, -1, 1) * maxStreerAngle;

        if (Mathf.Abs(steer) > 0.1f)
        {
            Quaternion deltaRotation = Quaternion.Euler(0, steer * Time.deltaTime, 0);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }
        Vector3 forwardMovement = HoldCarTransform.forward * accelerations * moveSpeed * Time.deltaTime;
        //Debug.Log(forwardMovement);
        _rigidbody.MovePosition(_rigidbody.position + forwardMovement);
    }

    public void Drive(float accelerations, float steer, float brake)
    {
        accelerations = Mathf.Clamp(accelerations, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxStreerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
        float thrustTorque = accelerations * torque;

        if (brake != 0)
            brakeLight.SetActive(true);
        else 
            brakeLight.SetActive(false);

        thrustTorque = Mathf.Clamp(thrustTorque, -maxSpeed, maxSpeed);
        for (int i = 0; i < 4; i++)
        {
            _wheelCollider[i].motorTorque = thrustTorque;

            if (i < 2)
            {
                _wheelCollider[i].steerAngle = steer;
            }
            else
            {
                _wheelCollider[i].brakeTorque = brake;
            }

            Quaternion quaternion;
            Vector3 position;
            _wheelCollider[i].GetWorldPose(out position, out quaternion);
            Wheels[i].transform.rotation = quaternion;
            Wheels[i].transform.position = position;
        }
    }

    public void HoldCarWalk(float vertical, float horizontal, float jump)
    {
        //float stateTimer = 5;
        vertical = Mathf.Clamp(vertical, -1, 1);
        horizontal = Mathf.Clamp(horizontal, -1, 1);

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            Quaternion deltaRotation = Quaternion.Euler(0, horizontal * rotationSpeed * Time.deltaTime, 0);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }

        Vector3 forwardMovement = HoldCarTransform.forward * vertical * moveSpeed * Time.deltaTime;
        _rigidbody.MovePosition(_rigidbody.position + forwardMovement);

        if (jump > 0 && IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //coolDownTimer = stateTimer;
        }
        //coolDownTimer -= Time.deltaTime;
    }

    void Update()
    {
        IsGrounded();
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(boxGroundCheck.position, Vector3.down, out hit, groundCheckDistance, ground);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(boxGroundCheck.position, boxGroundCheck.position + Vector3.down * groundCheckDistance);
    }

    public void SwitchModeCollider(bool state)
    {
        {
            if (state)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
            carCollider.enabled = !state;
            characterCollider.enabled = state;

            for (int i = 0; i < 4; i++)
            {
                _wheelCollider[i].enabled = !state;
            }
        }
    }

    public void SetLampActivate(bool state)
    {
        for (int i = 0; i < 2; i++)
        {
            Lamp[i].SetActive(state);
        }
    }
}