using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsManager : MonoBehaviour
{
    [Header("Prefab Lists")]
    public List<GameObject> roomPrefabs;
    public GameObject itemShopPrefab;
    public GameObject tutorialPrefab;
    public GameObject specialPlatformPrefab;

    [Header("Game Flow")]
    public int roomsUntilShop = 3;
    public float verticalSpawnOffset = 20f; // Vertical distance between rooms
    public float yOffset = 10f; // Public Y offset for the gap between room spawns

    private int roomsCounter = 0;
    private float highestPlatformY;

    void Start()
    {
        // Spawn the tutorial room at the specific position (62.7, 4, 0)
        Vector3 tutorialPosition = new Vector3(62.7f, 12.5f, 0f);
        SpawnRoom(tutorialPrefab, tutorialPosition);

        // Adjust the highestPlatformY for the next room spawn
        highestPlatformY = tutorialPosition.y + yOffset;
    }

    void Update()
    {
        CheckForNewRoomSpawn();
    }

    private void CheckForNewRoomSpawn()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.transform.position.y > highestPlatformY - yOffset)
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
            nextRoomPrefab = itemShopPrefab;
        }
        else
        {
            nextRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
        }

        // Calculate the new room's position using yOffset
        Vector3 spawnPosition = new Vector3(0, highestPlatformY + verticalSpawnOffset, 0);
        SpawnRoom(nextRoomPrefab, spawnPosition);

        // Update the highestPlatformY for the next room
        highestPlatformY += verticalSpawnOffset + yOffset;
    }

    private void SpawnRoom(GameObject roomPrefab, Vector3 position)
    {
        GameObject room = Instantiate(roomPrefab, position, Quaternion.identity);
        room.transform.parent = transform;

        // If it's the first room, update the highestPlatformY based on the room's content
        if (roomsCounter == 0)
        {
            highestPlatformY = GetHighestPlatformY(room);
        }
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
