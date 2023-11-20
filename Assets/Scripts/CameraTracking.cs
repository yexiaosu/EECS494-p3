using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    private Transform player;
    public Vector3 offset;

    [Tooltip("Higher values make the camera follow the player more quickly.")]
    public float followSpeedMultiplier = 2.0f;

    // Additional multipliers for specific scenarios
    public float fallingFollowMultiplier = 5.0f; // Multiplier for faster following when falling

    private Camera cam;

    void Start()
    {
        // Get the camera component
        cam = GetComponent<Camera>();

        // Find the player GameObject by tag and get its transform
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found. Ensure the player GameObject is tagged as 'Player'.");
        }
    }

    void LateUpdate()
    {
        if (player)
        {
            // Calculate the vertical position of the player in screen space
            float playerScreenPos = cam.WorldToViewportPoint(player.position).y;

            // Determine the target Y position based on player's position
            float targetY = player.position.y + offset.y;

            if (playerScreenPos > 0.66f)
            {
                // If the player is in the upper third, follow upwards more aggressively
                targetY = Mathf.Lerp(transform.position.y, targetY, 1.5f * followSpeedMultiplier * Time.deltaTime);
            }
            else if (playerScreenPos < 0.33f)
            {
                // If the player is in the lower third, follow downwards very aggressively
                targetY = Mathf.Lerp(transform.position.y, targetY, fallingFollowMultiplier * Time.deltaTime);
            }
            else
            {
                // If the player is in the middle third, maintain the current vertical position
                targetY = transform.position.y;
            }

            // Set the camera's position
            transform.position = new Vector3(transform.position.x + offset.x, targetY, transform.position.z);
        }
    }
}
