using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5.0f;
    private float jumpForce = 10.0f;
    private bool isJumping = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing");
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        // Horizontal Movement
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Jump Mechanic
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    // Detect collision with ground
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Ground>() != null)
        {
            isJumping = false;
        }
    }

    //added this to access speed and jump from external scripts
    public void changeSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void changeJump(float jump)
    {
        jumpForce = jump;
    }
}
