using System.Collections;
using UnityEngine;

public class EnemyWallCrawler : MonoBehaviour
{
    public GameObject projectilePrefab;
    public LayerMask whatIsWall;
    public float moveSpeed = 2f;
    public float shootCooldown = 2f;
    public float stopTimeBeforeShoot = 0.5f;
    public float raycastLength = 0.1f; // Increased raycast distance to avoid frequent direction changes

    private Rigidbody2D rb;
    private float shootTimer;
    private GameObject player;
    private Vector2 direction;
    private bool isShooting = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Ignore gravity
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player = GameObject.FindGameObjectWithTag("Player");
        direction = Vector2.up; // Start moving up
        shootTimer = shootCooldown;
    }

    private void Update()
    {
        // If currently shooting, don't do anything else
        if (isShooting) return;

        // Check if we should switch the direction (up/down)
        if (!isShooting && CheckForWall())
        {
            direction = -direction;
        }

        // Move up or down the wall
        rb.velocity = direction * moveSpeed;

        // Shooting logic
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            StartCoroutine(Shoot());
            shootTimer = shootCooldown; // Reset shoot timer
        }
    }

    private bool CheckForWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastLength, whatIsWall);
        return !hit.collider;
    }

    private IEnumerator Shoot()
    {
        // Stop and shoot
        isShooting = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(stopTimeBeforeShoot);

        // Instantiate and shoot the projectile towards the player
        Vector2 shootDir = (player.transform.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = shootDir * moveSpeed * 2; // Adjust projectile speed as necessary

        isShooting = false; // Resume moving
        yield return new WaitForSeconds(shootCooldown); // Shooting cooldown
    }

    private void OnDrawGizmos()
    {
        // This will draw a line in the editor to show the current direction of movement
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(direction.x, direction.y, 0) * raycastLength);
    }

    // Include Dead() and SpawnCollectible() methods from your original Enemy script
}
