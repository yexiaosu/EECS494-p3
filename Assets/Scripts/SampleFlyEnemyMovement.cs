using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class SampleFlyEnemyMovement : FlyMovement
{

    private Transform playerTransform;
    private float dashTimer = 0f;
    private float dashCooldown = 3f; // Time between dashes

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        time = 0;
        changeDirectionTime = 0;
        StartCoroutine(Fly());
        pauseEventSubscription = EventBus.Subscribe<PauseEvent>(_OnPause);
        resumeEventSubscription = EventBus.Subscribe<ResumeEvent>(_OnResume);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            // Calculate direction towards the player
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;
            if(distanceToPlayer > 15)
            {
                return;
            }
            directionToPlayer.Normalize();

            dashTimer += Time.deltaTime;

            if (dashTimer >= dashCooldown)
            {
                dashTimer = 0f;
                rb.velocity = directionToPlayer * speed * 3;
            }
            else if(dashTimer < .15f)
            {
                rb.velocity = directionToPlayer * speed * 3;
            }
            else
            {
                rb.velocity = directionToPlayer * speed;
            }


            
        }
        else
        {
            Move();
        }
    }
}
