using UnityEngine;

public class DelayedRotation : MonoBehaviour
{
    public Transform target;
    public float rotationLagSpeed = 3f;
    public float smoothFactor = 0.05f; // Yếu tố làm mượt.

    private Quaternion desiredRotation;

    void Start()
    {
        desiredRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        Quaternion targetRotation = target.rotation;

        desiredRotation = Quaternion.Lerp(desiredRotation, targetRotation, rotationLagSpeed * Time.smoothDeltaTime);


        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothFactor);
    }
}