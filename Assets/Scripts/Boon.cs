using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Boon : MonoBehaviour
{
    private float speedBoost = .5f;  // The additional speed the player gains
    private float jumpBoost = .1f;   // The additional jump force the player gains
    //public GameObject Canvas;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the PlayerMovement script
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player)
        {
            BoonText bt = collision.gameObject.GetComponent<BoonText>();
            // Increase player's speed and jump
            player.changeSpeed(player.GetMoveSpeed() + speedBoost);
            player.changeJump(player.GetJumpForce() + jumpBoost);

            bt.getBoon("Speed and Jump");
            
            // Destroy the boon
            Destroy(gameObject);
        }
    }
}
