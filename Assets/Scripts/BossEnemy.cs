using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public GameObject deadBossDoorPrefab; // Assign this prefab in the Inspector
    private Subscription<EnemyDeadEvent> enemyDeadEventSubscription;

    // Start is called before the first frame update
    void Start()
    {
        enemyDeadEventSubscription = EventBus.Subscribe<EnemyDeadEvent>(_OnEnemyDead);
    }

    private void SpawnDeadBossDoor()
    {
        if (deadBossDoorPrefab != null)
        {
            GameObject roomGridObject = GameObject.Find("Room Grid");
            if (roomGridObject != null)
            {
                Vector3 spawnPosition = transform.position - new Vector3(0, 12, 0); // Adjust the y-coordinate
                Instantiate(deadBossDoorPrefab, spawnPosition, Quaternion.identity, roomGridObject.transform);
            }
            else
            {
                Debug.LogError("Room Grid object not found in the scene.");
            }
        }
        else
        {
            Debug.LogError("Dead Boss Door Prefab is not set.");
        }
    }

    private void _OnEnemyDead(EnemyDeadEvent e)
    {
        SpawnDeadBossDoor();
    }
}
