using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    public enum BossState { Idle, Walking, Attacking, Damaged, Dead }
    public BossState currentState = BossState.Idle;

    public GameObject meleeHitEffect;
    public GameObject projectileHitEffect;
    public GameObject missileHitEffect;

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

    private GameObject meleeHitEffectObject;
    private GameObject projectileHitEffectObject;
    private GameObject missileHitEffectObject;

    private float timeToAttack = 0.25f;
    private bool isPlayingMeleeHitEffect = false;
    private bool isPlayingProjectileHitEffect = false;
    private bool isPlayingMissileHitEffect = false;
    private float timerMeleeAttack = 0;
    private float timerProjectileAttack = 0;
    private float timerMissileAttack = 0;

    public float rushSpeed = 16.0f;
    private bool isTransforming = false; // To control the transformation process
    public GameObject bossRoomStage2Prefab; // Assign this in the Inspector
    private GameObject roomGrid; // Assign or find this in Start()


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

        // Instantiate effect objects
        meleeHitEffectObject = Instantiate(meleeHitEffect, transform.position + new Vector3(0.4f, 0.3f, 0), Quaternion.identity);
        meleeHitEffectObject.transform.parent = transform;
        meleeHitEffectObject.SetActive(false);

        projectileHitEffectObject = Instantiate(projectileHitEffect, transform.position, Quaternion.identity);
        projectileHitEffectObject.transform.parent = transform;
        projectileHitEffectObject.SetActive(false);

        missileHitEffectObject = Instantiate(missileHitEffect, transform.position, Quaternion.identity);
        missileHitEffectObject.transform.parent = transform;
        missileHitEffectObject.SetActive(false);
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

        UpdateHitEffects();
    }

    void CheckHealth()
    {
        if (!healthSystem.IsAlive)
        {
            currentState = BossState.Dead;
        }
        else if (healthSystem.CurrentHealthPercentage <= 50 && !isTransforming)
        {
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

        // Stop moving and delete the Boss Room
        currentState = BossState.Idle;
        Destroy(GameObject.Find("Boss Room"));

        // Instantiate Boss Room Stage 2
        GameObject bossRoomStage2 = Instantiate(bossRoomStage2Prefab, roomGrid.transform);

        // Move enemies to the Enemies parent object
        GameObject enemiesParent = GameObject.Find("Enemies");
        foreach (Transform child in bossRoomStage2.transform)
        {
            if (child.GetComponent<Enemy>() != null)
            {
                child.SetParent(enemiesParent.transform);
            }
        }

        // Wait for all enemies to be defeated
        yield return new WaitUntil(() => AreAllEnemiesDefeated(enemiesParent));

        // Move back down and resume attacking
        Vector3 targetPosition = new Vector3(transform.position.x, 0, transform.position.z);
        float elapsedTime = 0;
        while (elapsedTime < 3f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, (elapsedTime / 3f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isTransforming = false;
        currentState = BossState.Idle;
    }

    private bool AreAllEnemiesDefeated(GameObject enemiesParent)
    {
        // Check if there are any Enemy components under the enemiesParent
        return enemiesParent.GetComponentsInChildren<Enemy>().Length == 0;
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

    // Methods for handling different types of hits
    public void GetMeleeHit()
    {
        meleeHitEffectObject.SetActive(true);
        isPlayingMeleeHitEffect = true;
    }

    public void GetProjectileHit(Vector3 attackPos)
    {
        projectileHitEffectObject.SetActive(true);
        projectileHitEffectObject.transform.position = attackPos;
        isPlayingProjectileHitEffect = true;
    }

    public void GetMissileHit(Vector3 vectorAttack, Vector3 attackPos)
    {
        missileHitEffectObject.SetActive(true);
        missileHitEffectObject.transform.position = attackPos;
        missileHitEffectObject.transform.rotation = Quaternion.identity;
        missileHitEffectObject.transform.RotateAround(missileHitEffectObject.transform.position, new Vector3(0, 0, 1), Mathf.Atan2(-vectorAttack.y, -vectorAttack.x) * Mathf.Rad2Deg);
        isPlayingMissileHitEffect = true;
    }

    void UpdateHitEffects()
    {
        if (isPlayingMeleeHitEffect)
        {
            timerMeleeAttack += Time.deltaTime;
            if (timerMeleeAttack >= timeToAttack)
            {
                timerMeleeAttack = 0f;
                isPlayingMeleeHitEffect = false;
                meleeHitEffectObject.SetActive(false);
            }
        }

        if (isPlayingProjectileHitEffect)
        {
            timerProjectileAttack += Time.deltaTime;
            if (timerProjectileAttack >= timeToAttack)
            {
                timerProjectileAttack = 0f;
                isPlayingProjectileHitEffect = false;
                projectileHitEffectObject.SetActive(false);
            }
        }

        if (isPlayingMissileHitEffect)
        {
            timerMissileAttack += Time.deltaTime;
            if (timerMissileAttack >= timeToAttack)
            {
                timerMissileAttack = 0f;
                isPlayingMissileHitEffect = false;
                missileHitEffectObject.SetActive(false);
            }
        }
    }

    public void Dead()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        animator.SetTrigger("Death");
        Destroy(gameObject, 1.25f);
    }
}
