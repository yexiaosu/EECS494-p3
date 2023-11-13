using UnityEngine;

public class VerticalParallax : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffectMultiplier;

    private float startPosition;
    private float length;

    void Start()
    {
        startPosition = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void LateUpdate()
    {
        float temp = (cameraTransform.position.y * (1 - parallaxEffectMultiplier));
        float dist = (cameraTransform.position.y * parallaxEffectMultiplier);

        transform.position = new Vector3(transform.position.x, startPosition + dist, transform.position.z);

        if (temp > startPosition + length) startPosition += length;
        else if (temp < startPosition - length) startPosition -= length;
    }
}
