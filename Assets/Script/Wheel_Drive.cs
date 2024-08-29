using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel_Drive : MonoBehaviour
{
    [SerializeField] private WheelCollider[] _wheelCollider;
    [SerializeField] private float torque = 80f;
    [SerializeField] private float maxStreerAngle = 30f;
    [SerializeField] private float maxBrakeTorque =500f;
    [SerializeField] private GameObject[] Wheels;
    [SerializeField] private GameObject[] Lamp;
    [SerializeField] private BoxCollider[] Collider;
    
    public float maxsSpeed = 150f;

    public Rigidbody _rigidbody;

    public float currentSpeed { get {  return _rigidbody.velocity.magnitude;} }

    private void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }
    void Start()
    {
        _rigidbody.centerOfMass = new Vector3(_rigidbody.centerOfMass.x, -0.5f, _rigidbody.centerOfMass.z);
    }

    public void Go(float accelerations, float steer, float brake)
    {
        accelerations = Mathf.Clamp(accelerations, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxStreerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
        float thrustTorque = accelerations * torque;

        thrustTorque = Mathf.Clamp(thrustTorque, -maxsSpeed, maxsSpeed);
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
            Wheels[i].transform.position = position;
            Wheels[i].transform.rotation = quaternion;
        }
    }

    void Update()
    {

    }

    public void SetLampActivate(bool state)
    {
        for (int i = 0; i < 2; i++)
        {
            Lamp[i].SetActive(state);
        }
    }
    
}
