using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceBossAI : MonoBehaviour
{
    public enum BossState { Idle, Moving, Melee, Ranged, Laser, Damaged, Dead }
    public BossState currentState = BossState.Idle;

    private Transform playerTransform;
    public float moveSpeed = 2.0f;
    public float meleeRange = 2.0f;
    private float actionCooldown = 3.0f;
    private float lastActionTime = 0;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private GameObject laserGameObject;
    private IceBossLaser iceBossLaserScript;
    public GameObject projectilePrefab; // Assign this in the Unity Editor
    public float projectileShootDelay = 0.5f;
    public Collider2D[] stageColliders;


    // Initialize other necessary components and variables
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the Z position of the boss to 0
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(currentPosition.x, currentPosition.y, 0);

        iceBossLaserScript = GetComponentInChildren<IceBossLaser>(true);
        if (iceBossLaserScript != null)
        {
            laserGameObject = iceBossLaserScript.gameObject;
        }
        else
        {
            Debug.LogError("IceBossLaser script not found on any child objects.");
        }
    }


    void Update()
    {
        if (currentState == BossState.Dead)
            return;

        if (Time.time - lastActionTime > actionCooldown)
        {
            DecideNextAction();
        }

        // Update the Walking speed in the animator
        UpdateWalkingAnimation();
    }

    private void FlipSpriteAndCollidersTowardsPlayer()
    {
        if (playerTransform != null)
        {
            bool shouldFlip = playerTransform.position.x < transform.position.x;
            spriteRenderer.flipX = shouldFlip;

            // Optionally, flip child sprites if needed
            foreach (Transform child in transform)
            {
                SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();
                if (childSprite != null)
                {
                    childSprite.flipX = shouldFlip;
                }

                // Flip colliders of the laser if this child is the laser
                IceBossLaser childLaserScript = child.GetComponent<IceBossLaser>();
                if (childLaserScript != null)
                {
                    childLaserScript.FlipColliders(shouldFlip);
                }
            }
        }
    }

    private void FlipCollider(Transform child, bool shouldFlip)
    {
        // Assuming the collider to flip is a BoxCollider2D
        BoxCollider2D collider = child.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            Vector2 offset = collider.offset;
            offset.x = shouldFlip ? -offset.x : offset.x; // Flip the offset
            collider.offset = offset;
        }

        // Add similar logic for other types of colliders if needed
    }

    void DecideNextAction()
    {
        FlipSpriteAndCollidersTowardsPlayer(); // Flip sprite before deciding action

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= meleeRange)
        {
            currentState = BossState.Melee;
            animator.SetTrigger("Melee");
        }
        else
        {
            int randomAction = Random.Range(0, 3); // Randomly pick an action
            switch (randomAction)
            {
                case 0:
                    currentState = BossState.Ranged;
                    animator.SetTrigger("Projectile");
                    ShootProjectileWithDelay();
                    break;
                case 1:
                    currentState = BossState.Laser;
                    animator.SetTrigger("Laser");
                    ActivateLaser();
                    break;
                case 2:
                    currentState = BossState.Moving;
                    StartCoroutine(MoveTowardsPlayer());
                    break;
            }
        }

        lastActionTime = Time.time;
    }



    void ShootProjectileWithDelay()
    {
        StartCoroutine(ShootProjectileAfterDelay());
    }
    private IEnumerator ShootProjectileAfterDelay()
    {
        yield return new WaitForSeconds(projectileShootDelay);

        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

        projectileInstance.transform.localScale = new Vector3(5f, 5f, 1f); // Scale the projectile

        IceBossProjectile projectileScript = projectileInstance.GetComponent<IceBossProjectile>();
        if (projectileScript != null)
        {
            float directionX = spriteRenderer.flipX ? -1f : 1f;
            projectileScript.Shoot(directionX);
        }
        else
        {
            Debug.LogError("IceBossProjectile script not found on the instantiated projectile.");
        }
    }

    void ActivateLaser()
    {
        if (laserGameObject != null)
        {
            Debug.Log("Laser");
            laserGameObject.SetActive(true); // Activate the laser GameObject
            iceBossLaserScript.ActivateLaser();
        }
        else
        {
            Debug.LogError("Laser GameObject is not assigned or not found.");
        }
    }




    IEnumerator MoveTowardsPlayer()
    {
        float elapsedTime = 0;
        float walkDuration = 3.0f;

        while (elapsedTime < walkDuration)
        {
            if (playerTransform != null)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                Vector3 newPosition = transform.position + new Vector3(direction.x, 0, 0) * moveSpeed * Time.deltaTime;
                rb.MovePosition(newPosition);

                // Flip sprite based on player's position
                FlipSpriteAndCollidersTowardsPlayer();
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        currentState = BossState.Idle;
    }


    private void UpdateWalkingAnimation()
    {
        if (currentState == BossState.Moving)
        {
            // When moving, set the Walking speed parameter in the animator to the moveSpeed
            animator.SetFloat("Walking", moveSpeed);
        }
        else
        {
            // When not moving, reset the Walking speed parameter in the animator
            animator.SetFloat("Walking", 0);
        }
    }



    public void TakeDamage(float damage)
    {
        var healthSystem = GetComponent<HealthSystemForDummies>();
        if (healthSystem != null && healthSystem.IsAlive)
        {
            healthSystem.AddToCurrentHealth(-damage);

            if (!healthSystem.IsAlive)
            {
                currentState = BossState.Dead;
                HandleDeath();
            }
            else
            {
                // If the boss is not dead, play the damaged animation and effects
                animator.SetTrigger("Damaged");
                var enemyComponent = GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.GetMeeleHit(); // Or other appropriate hit effect
                }
            }
        }
    }


    void ActivateMeleeCollider(int index)
    {
        for (int i = 0; i < stageColliders.Length; i++)
        {
            stageColliders[i].enabled = (i == index);
            Debug.Log("switched to " + index);
        }
    }

    void HandleDeath()
    {
        // Implement death logic, play death animation, etc.
        animator.SetTrigger("Dead");
        // Potentially destroy the boss game object or disable it
        // Destroy(gameObject, 1.25f); // For example
    }
}
