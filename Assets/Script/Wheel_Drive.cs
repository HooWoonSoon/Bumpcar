using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    public void Drive(float accelerations, float steer, float brake)
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
            Wheels[i].transform.rotation = quaternion;
            Wheels[i].transform.position = position;
        }
    }

    void Update()
    {

    }

    public void SwitchModeCollider(bool state)
    {
        Collider[0].center = new Vector3(0, 1.12f, 0);
        Collider[1].center = new Vector3(0, 0, -0.08008471f);
        for (int i = 0; i < 4; i++)
        {
            _wheelCollider[i].transform.position += new Vector3(0, 1.12f, 0);
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
