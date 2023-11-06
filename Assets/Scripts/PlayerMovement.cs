using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5.0f;
    private float jumpForce = 10.0f;
    private bool isJumping = false;
    private float jumpCast = .17f;
    private Subscription<PauseEvent> pauseEventSubscription;
    private Subscription<ResumeEvent> resumeEventSubscription;
    private Rigidbody2D rb;
    private float timer;
    private float timeToWait;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing");
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        pauseEventSubscription = EventBus.Subscribe<PauseEvent>(_OnPause);
        resumeEventSubscription = EventBus.Subscribe<ResumeEvent>(_OnResume);
    }

    void Update()
    {
        // Horizontal Movement
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        Vector2 boxSize = new Vector2(.25f, .2f);
        bool isGrounded = Physics2D.BoxCast(rb.position, boxSize, 0f, Vector2.down, jumpCast, LayerMask.GetMask("Platforms"));

        if (isGrounded && Time.time > timer)
        {
            isJumping = false;
        }

        // Jump Mechanic
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            timer = Time.time + .2f;
        }

    }

    // Detect collision with ground
    /*   This allowed player to jump when under or to side of blocks
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Ground>() != null)
        {
            isJumping = false;
        }
    }
    */

    //added this to access speed and jump from external scripts
    public void changeSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void changeJump(float jump)
    {
        jumpForce = jump;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }

    private void _OnPause(PauseEvent e)
    {
        this.enabled = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }

    private void _OnResume(ResumeEvent e)
    {
        this.enabled = true;
        rb.gravityScale = 1;
    }
}
