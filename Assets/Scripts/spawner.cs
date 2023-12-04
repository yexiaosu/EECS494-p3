using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnInterval = 5f;
    public float spawnDistance = 5f;

    private float timer;

    void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if the timer has reached the spawn interval
        if (timer >= spawnInterval)
        {
            // Reset the timer
            timer = 0f;

            // Check if the player is within the specified y distance
            if (Mathf.Abs(transform.position.y - Camera.main.transform.position.y) <= spawnDistance)
            {
                // Spawn prefab to the left of the player
                Vector3 leftSpawnPosition = transform.position - Vector3.right;
                Instantiate(prefabToSpawn, leftSpawnPosition, Quaternion.identity);

                // Spawn prefab to the right of the player
                Vector3 rightSpawnPosition = transform.position + Vector3.right;
                Instantiate(prefabToSpawn, rightSpawnPosition, Quaternion.identity);
            }
        }
    }
}
