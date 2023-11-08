using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsManager : MonoBehaviour
{
    [Header("Prefab Lists")]
    public List<GameObject> roomPrefabs; // List of room prefabs to spawn
    public GameObject itemShopPrefab; // Prefab for the item shop room
    public GameObject tutorialPrefab; // Prefab for the tutorial room
    public GameObject specialPlatformPrefab; // Prefab for the special platform

    [Header("Game Flow")]
    public int roomsUntilShop = 3;
    public float verticalSpawnOffset = 20f; // Vertical distance between rooms

    private int roomsCounter = 0; // Counter to track the number of spawned rooms
    private float highestPlatformY; // Y position of the highest platform in the current room

    void Start()
    {
        // Spawn the tutorial room first
        SpawnRoom(tutorialPrefab, Vector3.zero);
    }

    void Update()
    {
        // Call your method to check if the player has reached a new room threshold here
        // For example, using player's Y position or score
        CheckForNewRoomSpawn();
    }

    private void CheckForNewRoomSpawn()
    {
        // Placeholder condition for when to spawn a new room
        // You might want to check the player's Y position or a score instead
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.transform.position.y > highestPlatformY - 5f) // 5 units before reaching the highest platform
        {
            SpawnNextRoom();
        }
    }

    private void SpawnNextRoom()
    {
        roomsCounter++;

        GameObject nextRoomPrefab;
        if (roomsCounter % roomsUntilShop == 0)
        {
            // Every 'roomsUntilShop' rooms, spawn an item shop
            nextRoomPrefab = itemShopPrefab;
        }
        else
        {
            // Otherwise, spawn a random regular room
            nextRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
        }

        // Calculate the new room's position
        Vector3 spawnPosition = new Vector3(0, highestPlatformY + verticalSpawnOffset, 0);
        SpawnRoom(nextRoomPrefab, spawnPosition);

        // Spawn the special platform at the bottom of the new room
        SpawnSpecialPlatform(spawnPosition - new Vector3(0, verticalSpawnOffset / 2, 0));
    }

    private void SpawnRoom(GameObject roomPrefab, Vector3 position)
    {
        GameObject room = Instantiate(roomPrefab, position, Quaternion.identity);
        room.transform.parent = transform;

        // Update the highest platform Y position for next spawn
        highestPlatformY = GetHighestPlatformY(room);
    }

    private void SpawnSpecialPlatform(Vector3 position)
    {
        Instantiate(specialPlatformPrefab, position, Quaternion.identity);
    }

    private float GetHighestPlatformY(GameObject room)
    {
        // Get all platforms in the room and return the highest Y position
        Platform[] platforms = room.GetComponentsInChildren<Platform>();
        float maxY = float.MinValue;
        foreach (Platform platform in platforms)
        {
            if (platform.transform.position.y > maxY)
            {
                maxY = platform.transform.position.y;
            }
        }
        return maxY;
    }
}
