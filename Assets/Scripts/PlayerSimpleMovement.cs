using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimpleMovement : MonoBehaviour
{
    public bool AbleToTriggerDialog = false;
    public GameObject Dialog;

    private float moveSpeed = 4.0f;
    private Rigidbody2D rb;
    private float jumpCast = .17f;
    private float jumpForce = 5.5f;
    private bool isApplyingJumping = false;
    private float maxJumpingTime = 0.5f;
    private float jumpTimer = 0f;
    private TrailRenderer tr;
    private bool AbleToJump = false;

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
    }

    void Update()
    {
        // Horizontal Movement
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        Vector2 boxSize = new Vector2(.25f, .2f);
        bool isGrounded = Physics2D.BoxCast(rb.position, boxSize, 0f, Vector2.down, jumpCast, LayerMask.GetMask("Platforms"));

        if (AbleToTriggerDialog && Input.GetKeyDown(KeyCode.F))
        {
            Dialog.SetActive(true);
            Dialog.transform.Find("Dialog").Find("1").gameObject.SetActive(true);
            Dialog.transform.Find("miko").gameObject.SetActive(true);
        }

        //// Jump Mechanic
        if (Input.GetButtonDown("Jump") && AbleToJump)
        {
            if (isGrounded)
            {
                isApplyingJumping = true;
                tr.emitting = true;
                jumpTimer = maxJumpingTime;
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        if (Input.GetButton("Jump") && isApplyingJumping && AbleToJump)
        {
            if (jumpTimer > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.5f);
                jumpTimer -= Time.deltaTime;
            }
            else
            {
                isApplyingJumping = false;
                tr.emitting = false;
            }
        }
        if (Input.GetButtonUp("Jump") && AbleToJump)
        {
            isApplyingJumping = false;
            tr.emitting = false;
        }
    }

    public void SetAbleToJump()
    {
        AbleToJump = true;
    }
}
