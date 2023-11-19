using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool DoubleJumpEnabled = false;
    public bool DashEnabled = false;
    public float DashCd = 1.0f;

    private float moveSpeed = 3.5f;
    private float jumpForce = 10.2f;
    private bool canDoubleJump = false;
    private float jumpCast = .17f;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashingPower = 25.0f;
    private float dashingTime = 0.2f;
    private Subscription<PauseEvent> pauseEventSubscription;
    private Subscription<ResumeEvent> resumeEventSubscription;
    private Rigidbody2D rb;
    private TrailRenderer tr;
    private float timer;
    private float timeToWait;

    [SerializeField] private AudioSource jumpSoundEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing");
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        tr = GetComponent<TrailRenderer>();
        pauseEventSubscription = EventBus.Subscribe<PauseEvent>(_OnPause);
        resumeEventSubscription = EventBus.Subscribe<ResumeEvent>(_OnResume);
    }

    void Update()
    {
        if (isDashing)
            return;
        // Horizontal Movement
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        Vector2 boxSize = new Vector2(.25f, .2f);
        bool isGrounded = Physics2D.BoxCast(rb.position, boxSize, 0f, Vector2.down, jumpCast, LayerMask.GetMask("Platforms"));

        if (isGrounded && Time.time > timer && DoubleJumpEnabled)
        {
            canDoubleJump = true;
        }

        //// Jump Mechanic
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded && Time.time > timer)
            {
                jumpSoundEffect.Play();
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                timer = Time.time + .2f;
            }
            else if (Time.time > timer)
            {
                if (DoubleJumpEnabled && canDoubleJump)
                {
                    jumpSoundEffect.Play();
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    canDoubleJump = false;
                }
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) && canDash && DashEnabled)
        {
            StartCoroutine(Dash(new Vector2(rb.velocity.x, 0).normalized));
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

    private IEnumerator Dash(Vector2 dir)
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = dir * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(DashCd);
        canDash = true;
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
