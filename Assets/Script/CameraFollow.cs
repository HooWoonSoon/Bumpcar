using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset;   
    public float smoothSpeed = 0.125f;  
    public float rotationSpeed = 5.0f;  

    private void FixedUpdate()
    {
        
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
 
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        Quaternion targetRotation = Quaternion.LookRotation(target.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
