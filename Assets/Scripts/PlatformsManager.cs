using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsManager : MonoBehaviour
{
    [Header("Prefab Lists")]
    public List<GameObject> roomPrefabs;
    public GameObject itemShopPrefab;
    public GameObject tutorialPrefab;
    public Transform roomGrid;
    public GameObject transitionPlatformPrefab;

    [Header("Game Flow")]
    public int roomsUntilShop = 5; // Item shop spawns every 5 rooms
    public float verticalSpawnOffset = 20f; // Vertical distance between rooms

    private int roomsCounter = 0;
    private float highestPlatformY;
    private float transitionPlatformY; // Y position of the current transition platform

    private void Start()
    {
        // Spawn the tutorial room first
        Vector3 tutorialPosition = new Vector3(62.7f, 12.5f, 0f);
        GameObject tutorialRoom = SpawnRoom(tutorialPrefab, tutorialPosition);

        // Calculate the highest platform of the tutorial room
        highestPlatformY = GetHighestPlatformY(tutorialRoom);

        // Spawn the first transition platform
        SpawnTransitionPlatform();
    }

    void Update()
    {
        CheckForNewRoomSpawn();
    }

    private void CheckForNewRoomSpawn()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // Spawn the next room when the player reaches the transition platform
        if (player.transform.position.y >= transitionPlatformY)
        {
            SpawnNextRoom();
        }
    }

    private GameObject SpawnRoom(GameObject roomPrefab, Vector3 position)
    {
        GameObject room = Instantiate(roomPrefab, position, Quaternion.identity);
        room.transform.SetParent(roomGrid, false);
        return room;
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

        // Spawn the next room above the current highest platform
        Vector3 spawnPosition = new Vector3(0, highestPlatformY + verticalSpawnOffset, 0);
        GameObject spawnedRoom = SpawnRoom(nextRoomPrefab, spawnPosition);

        // Calculate the new highest platform Y position
        highestPlatformY = GetHighestPlatformY(spawnedRoom);

        // Spawn a new transition platform for the next room, if it's not an item shop
        if (roomsCounter % roomsUntilShop != 0)
        {
            SpawnTransitionPlatform();
        }
    }

    private float GetHighestPlatformY(GameObject room)
    {
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

    private void SpawnTransitionPlatform()
    {
        // Calculate the spawn position for the next transition platform
        Vector3 transitionPlatformPosition = new Vector3(-1.7f, highestPlatformY + verticalSpawnOffset - 1f, 0);
        SpawnRoom(transitionPlatformPrefab, transitionPlatformPosition);

        // Update the Y position of the next transition platform
        transitionPlatformY = transitionPlatformPosition.y + 1f; // Adjust to account for the platform's height
    }
}
