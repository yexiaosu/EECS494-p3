using UnityEngine;

public class VerticalParallax : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffectMultiplier;
    public float smoothness = 10f; // Adjust this value to control the movement speed

    private float lastCameraY;

    void Start()
    {
        lastCameraY = cameraTransform.position.y;
    }

    void LateUpdate()
    {
        float deltaY = (cameraTransform.position.y - lastCameraY) * parallaxEffectMultiplier;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + deltaY, transform.position.z);

        // Interpolate towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);

        lastCameraY = cameraTransform.position.y;
    }
}
