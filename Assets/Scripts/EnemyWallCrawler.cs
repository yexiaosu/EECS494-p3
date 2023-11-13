using System.Collections;
using UnityEngine;

public class EnemyWallCrawler : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float moveSpeed = 2f;
    public float shootCooldown = 1.5f;
    public float detectionRange = 10f;
    public Vector2 initialDirectionTowardsWall;

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
            rb.velocity = moveDirection * moveSpeed;
        }

        shootTimer -= Time.deltaTime;

        if (PlayerInSight() && shootTimer <= 0)
        {
            Debug.Log("Player in sight, attempting to shoot."); // Debug line
            StartCoroutine(Shoot());
            shootTimer = shootCooldown;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Wall>())
        {
            isTouchingWall = true;
            moveDirection = Vector2.up;
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, detectionRange);
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red); 
        return hit.collider != null && hit.collider.gameObject == player;
    }

    private void MoveAlongWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 0.1f);
        if (hit.collider == null || !hit.collider.gameObject.GetComponent<Wall>())
        {
            moveDirection = -moveDirection;
        }

        rb.velocity = moveDirection * moveSpeed;
    }

    private IEnumerator Shoot()
    {
        Debug.Log("Shooting coroutine started.");
        isShooting = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.5f);

        Debug.Log("Shooting now."); 
        Vector2 shootDir = (player.transform.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = shootDir * moveSpeed * 2;

        yield return new WaitForSeconds(1f);
        isShooting = false;
    }

}
