using UnityEngine;

public class IceBossProjectile : MonoBehaviour
{
    public float speed = 5f; // Speed of the projectile

    void Start()
    {
        // Ensure the projectile's sprite is initially not flipped
        GetComponent<SpriteRenderer>().flipY = false;
    }

    public void Shoot(float directionX)
    {
        // Flip the sprite on the Y-axis if shooting towards the positive X direction
        GetComponent<SpriteRenderer>().flipY = directionX > 0;

        // Set the initial velocity of the projectile
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(speed * directionX, 0);
        }
    }

    void Update()
    {
        // Optional: Add logic for what happens as the projectile moves (like auto-destruction after some time)
    }

    // You can add collision detection methods here if needed
}
