using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    public enum BossState { Idle, Walking, Attacking, Damaged, Dead }
    public BossState currentState = BossState.Idle;

    private Transform playerTransform; // Assign the player's transform here
    public float moveSpeed = 3.0f;
    public float attackRange = 5.0f;
    public float meleeAttackCooldown = 2.0f;
    public float rangedAttackCooldown = 3.0f;
    private float lastAttackTime = 0;

    private Animator animator;
    private HealthSystemForDummies healthSystem;
    private bool isPhaseTwo = false;
    private bool isEnraged = false;
    private float invulnerabilityTime = 1.0f;
    private float lastDamageTime;

    private float timeToAttack = 0.25f;

    public float rushSpeed = 16.0f;
    private bool isTransforming = false; // To control the transformation process
    public GameObject bossRoomStage2Prefab; // Assign this in the Inspector
    private GameObject roomGrid; // Assign or find this in Start()
    private bool transformationTriggered = false;


    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure your player GameObject is tagged correctly.");
        }
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystemForDummies>();
        roomGrid = GameObject.Find("Room Grid");

        
    }

    void Update()
    {
        switch (currentState)
        {
            case BossState.Idle:
                CheckHealth();
                DecideNextAction();
                break;
            case BossState.Walking:
                CheckHealth();
                MoveTowardsPlayer();
                DecideNextAction(); // Reevaluate action
                break;
            case BossState.Attacking:
                CheckHealth();
                PerformAttack();
                break;
            case BossState.Damaged:
                if (Time.time - lastDamageTime > invulnerabilityTime)
                {
                    currentState = BossState.Idle; // Reset to idle after invulnerability
                    animator.SetBool("Damaged", false);
                }
                break;
            case BossState.Dead:
                // Death behavior
                animator.SetBool("Death", true);
                break;
        }

    }

    void CheckHealth()
    {
        if (!healthSystem.IsAlive)
        {
            currentState = BossState.Dead;
        }
        else if (healthSystem.CurrentHealthPercentage <= 50 && !transformationTriggered)
        {
            transformationTriggered = true; // Set the flag
            StartCoroutine(TransformAndDestroyPlatforms());
        }
        else if (healthSystem.CurrentHealthPercentage <= 20 && !isEnraged)
        {
            EnterEnragedState();
        }
    }

    IEnumerator TransformAndDestroyPlatforms()
    {
        isTransforming = true;
        currentState = BossState.Idle;

        // Move to x = 0, y += 3.5
        Vector3 initialTargetPosition = new Vector3(0, transform.position.y + 3.5f, transform.position.z);
        float elapsedTime = 0;
        while (elapsedTime < 3f)
        {
            transform.position = Vector3.Lerp(transform.position, initialTargetPosition, (elapsedTime / 3f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (Transform child in roomGrid.transform)
        {
            if (child.name.Contains("Boss Room"))
            {
                Destroy(child.gameObject);
                break; // Exit the loop once the boss room is found and destroyed
            }
        }

        // Instantiate Boss Room Stage 2
        Vector3 spawnPosition = new Vector3(0f, playerTransform.position.y - 10, playerTransform.position.z);
        Instantiate(bossRoomStage2Prefab, spawnPosition, Quaternion.identity, roomGrid.transform);

        // Move enemies to the Enemies parent object
        GameObject enemiesParent = GameObject.Find("Enemies");
        foreach (Transform child in bossRoomStage2Prefab.transform)
        {
            if (child.GetComponent<Enemy>() != null)
            {
                child.SetParent(enemiesParent.transform);
            }
        }

        // Teleport boss to new position above the player
        Vector3 teleportPosition = new Vector3(0f, playerTransform.position.y + 20f, transform.position.z);
        transform.position = teleportPosition;

        currentState = BossState.Walking;
        isTransforming = false;
    }



    void EnterPhaseTwo()
    {
        isPhaseTwo = true;
        moveSpeed *= 1.5f;
        meleeAttackCooldown /= 2;
        rangedAttackCooldown /= 2;
    }

    void EnterEnragedState()
    {
        isEnraged = true;
        moveSpeed *= 2;
    }

    void DecideNextAction()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer < attackRange)
        {
            currentState = BossState.Attacking;
        }
        else
        {
            currentState = BossState.Walking;
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Calculate the absolute distance on the x-axis
        float xDistance = Mathf.Abs(transform.position.x - playerTransform.position.x);

        // Check if the boss should rush towards the player
        if (xDistance > 5f)
        {
            transform.position += direction * rushSpeed * Time.deltaTime; // Use rush speed
        }
        else
        {
            transform.position += direction * moveSpeed * Time.deltaTime; // Use normal speed
        }

        // Flip the sprite based on player position
        if (playerTransform.position.x > transform.position.x)
        {
            // Player is to the right, flip sprite
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // Player is to the left, use normal orientation
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        animator.SetBool("Walking", true);
    }


    void PerformAttack()
    {
        if (Time.time - lastAttackTime > meleeAttackCooldown)
        {
            animator.SetBool("Attacking", true);
            lastAttackTime = Time.time;
        }
        else
        {
            currentState = BossState.Idle; // Transition out of attacking state
            animator.SetBool("Attacking", false);
        }
    }

    public void TakeDamage()
    {
        if (Time.time - lastDamageTime > invulnerabilityTime)
        {
            lastDamageTime = Time.time;
            currentState = BossState.Damaged;
            animator.SetBool("Damaged", true);
        }
    }

    public void Dead()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        animator.SetTrigger("Death");
        Destroy(gameObject, 1.25f);
       
    }
}
