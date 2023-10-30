using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boon : MonoBehaviour
{
    public float speedBoost = 2.0f;  // The additional speed the player gains
    public float jumpBoost = 5.0f;   // The additional jump force the player gains

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the PlayerMovement script
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player)
        {
            // Increase player's speed and jump
            player.changeSpeed(player.GetMoveSpeed() + speedBoost);
            player.changeJump(player.GetJumpForce() + jumpBoost);

            // Destroy the boon
            Destroy(gameObject);
        }
    }
}
