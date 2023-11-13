using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public GameObject bossRoomPrefab; // Assign in Inspector
    private UIManager uiManager;
    private Transform roomGridTransform;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        // Find the Room Grid in the scene
        GameObject roomGridObject = GameObject.Find("Room Grid");
        if (roomGridObject != null)
        {
            roomGridTransform = roomGridObject.transform;
        }
        else
        {
            Debug.LogError("Room Grid object not found in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RoomManager roomManager = FindObjectOfType<RoomManager>();
            if (roomManager != null)
            {
                roomManager.ToggleRoomSpawning(false); // Stop spawning rooms
            }

            TeleportPlayer();
            SpawnBossRoom();
        }
    }


    private void TeleportPlayer()
    {
        // Teleport the player to a position based on the current score
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            int currentScore = uiManager.GetScore();
            player.transform.position = new Vector3(0, currentScore + 25f, 0);
        }
    }



    private void SpawnBossRoom()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && bossRoomPrefab != null && roomGridTransform != null)
        {
            // Spawn the Boss Room 5 units below the player's position
            Vector3 spawnPosition = new Vector3(0, player.transform.position.y - 15f, 0);
            Instantiate(bossRoomPrefab, spawnPosition, Quaternion.identity, roomGridTransform);
            Debug.Log("Boss Room spawned at: " + spawnPosition);
        }
        else
        {
            Debug.LogError("Boss Room Prefab, Room Grid Transform, or Player not found.");
        }
    }

}
