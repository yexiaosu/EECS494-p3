using System.Collections.Generic;
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 currentPosition = player.transform.position;
            player.transform.position = new Vector3(currentPosition.x, currentPosition.y + 25f, currentPosition.z);
        }
    }



    private void SpawnBossRoom()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && bossRoomPrefab != null && roomGridTransform != null)
        {
            // Spawn the Boss Room 5 units below the player's position
            Vector3 spawnPosition = new Vector3(0, player.transform.position.y - 15f, 0);
            GameObject roomInstance = Instantiate(bossRoomPrefab, spawnPosition, Quaternion.identity, roomGridTransform);
            Debug.Log("Boss Room spawned at: " + spawnPosition);
            if (roomInstance.transform.childCount > 0)
            {
                List<Transform> enemies = new List<Transform>();
                for (int i = 0; i < roomInstance.transform.GetChild(0).childCount; i++)
                {
                    Transform child = roomInstance.transform.GetChild(0).GetChild(i);
                    enemies.Add(child);
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].parent = GameObject.Find("Enemies").transform;
                }
            }
        }
        else
        {
            Debug.LogError("Boss Room Prefab, Room Grid Transform, or Player not found.");
        }
    }
}