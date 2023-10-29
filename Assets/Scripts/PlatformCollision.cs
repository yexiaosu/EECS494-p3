using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    private bool eventSent = false; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (eventSent) 
            return;

        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player)
        {
            // Check if collision happened at the top of the platform
            foreach (ContactPoint2D point in collision.contacts)
            {
                // it means the player hit the top of the platform.
                if (point.normal.y < -0.9f)
                {
                    EventBus.Publish(new PlayerTouchedPlatformEvent());
                    eventSent = true; // mark the event as sent
                    break;
                }
            }
        }
    }
}
