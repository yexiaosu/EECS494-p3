using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boneAttack : MonoBehaviour
{
    public GameObject rotatingPrefab; // Assign your rotating prefab in the Inspector
    private Transform playerTransform; // Assign the player's transform in the Inspector
    public float spawnCooldown = 3.5f; // Time between spawns
    private float spawnTimer = 0f;
    private Subscription<PauseEvent> pauseEventSubscription;
    private Subscription<ResumeEvent> resumeEventSubscription;

    void Start()
    {
        // Set the playerTransform reference, replace "Player" with the actual tag or name of your player object
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found!");
        }
        pauseEventSubscription = EventBus.Subscribe<PauseEvent>(_OnPause);
        resumeEventSubscription = EventBus.Subscribe<ResumeEvent>(_OnResume);
    }
    void Update()
    {
        SpawnPrefabCooldown();
    }

    void SpawnPrefabCooldown()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnCooldown)
        {
            SpawnRotatingPrefab();
            spawnTimer = 0f;
        }
    }

    void SpawnRotatingPrefab()
    {
        if (rotatingPrefab != null && playerTransform != null)
        {
            // Calculate direction towards the player
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            float distanceToPlayer = playerTransform.position.y - transform.position.y;

            if (distanceToPlayer <= 5f && distanceToPlayer >= -5f)
            {
                directionToPlayer.Normalize();

                // Instantiate the rotating prefab
                GameObject rotatingObject = Instantiate(rotatingPrefab, transform.position, Quaternion.identity);

                // Get the Rigidbody2D component of the rotating prefab
                Rigidbody2D rotatingRb = rotatingObject.GetComponent<Rigidbody2D>();

                // Set the velocity of the rotating prefab towards the player
                rotatingRb.velocity = directionToPlayer * 5;
            }

        }
        else
        {
            Debug.LogError("Prefab or player transform not assigned!");
        }
    }

    private void _OnPause(PauseEvent e)
    {
        enabled = false;
    }

    private void _OnResume(ResumeEvent e)
    {
        enabled = true;
    }
}
