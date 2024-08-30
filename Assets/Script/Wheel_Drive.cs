using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Wheel_Drive : MonoBehaviour
{
    [Header("Drive")]
    [SerializeField] private WheelCollider[] _wheelCollider;
    [SerializeField] private float torque = 80f;
    [SerializeField] private float maxStreerAngle = 30f;
    [SerializeField] private float maxBrakeTorque =500f;
    [SerializeField] private GameObject[] Wheels;
    [SerializeField] private GameObject[] Lamp;
    public float maxSpeed = 150f;

    public Rigidbody _rigidbody;

    [Header("Walk")]
    [SerializeField] private BoxCollider carCollider;
    [SerializeField] private BoxCollider characterCollider;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 100f;

    private Vector3 carOriginColliderCenter;
    private Vector3 characterOriginColliderCenter;
    private Vector3 movementDirection;

    public float currentSpeed { get {  return _rigidbody.velocity.magnitude;} }

    private void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }
    void Start()
    {
        _rigidbody.centerOfMass = new Vector3(_rigidbody.centerOfMass.x, -0.5f, _rigidbody.centerOfMass.z);
    }

    public void Drive(float accelerations, float steer, float brake)
    {
        accelerations = Mathf.Clamp(accelerations, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxStreerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
        float thrustTorque = accelerations * torque;

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

    public void HoldCarWalk(float vertical, float horizontal, float jump) // To controt character movement and rotation, but the operation base would rectify by transform orientation
    {
        vertical = Mathf.Clamp(vertical, -1, 1);
        horizontal = Mathf.Clamp(horizontal, -1, 1);
        movementDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, toRotation, rotationSpeed * Time.deltaTime));
        }

        _rigidbody.MovePosition(_rigidbody.position + movementDirection * moveSpeed * Time.deltaTime);
    }

    void Update()
    {

    }

    public void SwitchModeCollider(bool state)
    {
        if (state)
        {
            carCollider.center = new Vector3(0, 1.12f, 0);
            characterCollider.center = new Vector3(0, 0, -0.08008471f);
            for (int i = 0; i < 4; i++)
            {
                _wheelCollider[i].transform.position += new Vector3(0, 1.12f, 0);
            }
        }
        else
        {
            carCollider.center = carOriginColliderCenter;
            characterCollider.center = characterOriginColliderCenter;
            for (int i = 0; i < 4; i++)
            {
                _wheelCollider[i].transform.position -= new Vector3(0, 1.12f, 0);
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
