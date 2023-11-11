using System.Collections;
using UnityEngine;

public class EnemyWallCrawler : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float moveSpeed = 2f;
    public float shootCooldown = 1.5f;
    public float detectionRange = 10f; // Range to detect the player
    public Vector2 initialDirectionTowardsWall; // Direction towards the nearest wall

    private Rigidbody2D rb;
    private GameObject player;
    private Vector2 moveDirection;
    private bool isShooting = false;
    private float shootTimer;
    private bool isTouchingWall = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player = GameObject.FindGameObjectWithTag("Player");

        // Initialize movement direction towards the wall
        moveDirection = initialDirectionTowardsWall.normalized;
    }

    private void Update()
    {
        if (isShooting) return;

        if (isTouchingWall)
        {
            MoveAlongWall();
        }
        else
        {
            // Move towards the wall
            rb.velocity = moveDirection * moveSpeed;
        }

        shootTimer -= Time.deltaTime;

        if (PlayerInSight() && shootTimer <= 0)
        {
            StartCoroutine(Shoot());
            shootTimer = shootCooldown;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collided with the wall
        if (collision.gameObject.GetComponent<Wall>())
        {
            isTouchingWall = true;
            // Change direction to move up initially
            moveDirection = Vector2.up;
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, detectionRange);
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red); // For debugging
        return hit.collider != null && hit.collider.gameObject == player;
    }

    private void MoveAlongWall()
    {
        // Check for collision with wall ends or corners to change direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 0.1f);
        if (hit.collider == null || !hit.collider.gameObject.GetComponent<Wall>())
        {
            // Reverse direction
            moveDirection = -moveDirection;
        }

        rb.velocity = moveDirection * moveSpeed;
    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.5f);

        Vector2 shootDir = (player.transform.position - transform.position).normalized;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = shootDir * moveSpeed * 2;

        yield return new WaitForSeconds(1f); // Delay before resuming movement
        isShooting = false;
    }

    // OnDrawGizmos remains unchanged
    private void OnDrawGizmos()
    {
        // Your existing Gizmos code
    }
}
