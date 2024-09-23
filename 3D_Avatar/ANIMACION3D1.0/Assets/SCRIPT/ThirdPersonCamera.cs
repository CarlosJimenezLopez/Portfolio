using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float damping = 10.0f;
    public float rotationDamping = 10.0f;

    void LateUpdate()
    {
        // Calculate camera position
        Vector3 targetPosition = target.position + target.forward * distance;
        targetPosition.y += height;
        targetPosition += target.right * distance * 0.2f;

        // Smoothly move camera to new position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * damping);

        // Rotate camera to face target
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationDamping);
    }
}
