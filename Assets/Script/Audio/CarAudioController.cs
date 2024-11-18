using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudioController : MonoBehaviour
{
    public AudioSource EngineAudio; 
    public AudioSource IdleAudio; 
    public Rigidbody carRigidbody; 
    public float minPitch = 0.5f; 
    public float maxPitch = 2.0f; 
    public float maxSpeed = 100f; 

    void Start()
    {
        if (EngineAudio == null)
            EngineAudio = GetComponent<AudioSource>();

        if (IdleAudio == null)
            IdleAudio = gameObject.AddComponent<AudioSource>();

        if (carRigidbody == null)
            carRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Calculate the current speed of the car
        float speed = carRigidbody.velocity.magnitude;

        if (speed > 0.1f)
        {
            // The car is moving, play the engine audio and stop the idle audio
            if (!EngineAudio.isPlaying)
            {
                EngineAudio.Play();
                IdleAudio.Stop();
            }

            // Adjust the pitch based on the speed
            float pitch = Mathf.Lerp(minPitch, maxPitch, speed / maxSpeed);
            EngineAudio.pitch = pitch;
        }
        else
        {
            // The car is idle, so play the idle audio and stop the engine audio
            if (!IdleAudio.isPlaying)
            {
                IdleAudio.Play();
                EngineAudio.Stop();
            }
        }
    }
}
