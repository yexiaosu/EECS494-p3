using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> roomPrefabs;
    public GameObject transitionPlatformPrefab;
    private bool stopSpawning = false;
    public float fixedRoomOffset = 0f;
    public GameObject bossDoorPrefab;
    public Tilemap platformTilemap; // Assign this in the Unity Inspector

    public float spawnThreshold = 10f; // Customizable threshold

    private GameObject currentRoom;
    private int roomCount = 0;
    private Transform roomGridTransform;
    private GameObject lastSpawnedPlatform;
    public GameObject afterBossPrefab;
    private bool bossRoomDoorSpawned = false;

    void Start()
    {
        EventBus.Subscribe<BossKilledEvent>(OnBossKilled);
        // Find the Room Grid in the scene
        GameObject roomGridObject = GameObject.Find("Room Grid");
        if (roomGridObject != null)
        {
            roomGridTransform = roomGridObject.transform;
        }
        else
        {
            Debug.LogError("Room Grid object not found in the scene.");
            return;
        }
        // Find the Player GameObject and get its Y-coordinate
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found in the scene.");
            return;
        }
        float playerY = player.transform.position.y;

        // Set the tutorial room position to the player's Y-coordinate, not relative to it
        Vector3 tutorialRoomPosition = new Vector3(0f, 0f, 0f);
        GameObject tutorialRoomPrefab = roomPrefabs[0];
        SpawnRoomAt(tutorialRoomPosition, true, tutorialRoomPrefab);
    }

    private void Update()
    {
        if (!stopSpawning && currentRoom != null && PlayerNearEndOfRoom())
        {
            Debug.Log("Spawning Transition Platform");
            SpawnTransitionPlatform();
        }
    }

    public void ToggleRoomSpawning(bool shouldSpawn)
    {
        stopSpawning = !shouldSpawn;
    }

    private bool PlayerNearEndOfRoom()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            BoundsInt bounds = platformTilemap.cellBounds;
            float upperBoundary = platformTilemap.CellToWorld(new Vector3Int(0, bounds.yMax, 0)).y;
            float playerY = player.transform.position.y;

            //Debug.Log($"Player Y position: {playerY}, Upper boundary: {upperBoundary}, Threshold: {spawnThreshold}");
            return playerY > (upperBoundary - spawnThreshold);
        }
        return false;
    }

    private GameObject GetRandomRoomPrefab()
    {
        int randomIndex = Random.Range(0, roomPrefabs.Count);
        return roomPrefabs[randomIndex];
    }

    private void SpawnTransitionPlatform()
{
    if (stopSpawning) return; // Stop spawning if the flag is set

    Vector3 highestPlatformPosition = FindHighestPlatformPosition();
    Vector3 transitionPosition = new Vector3(0f, highestPlatformPosition.y - 8f, 0f);

    lastSpawnedPlatform = Instantiate(transitionPlatformPrefab, transitionPosition, Quaternion.identity);
    if (roomGridTransform != null)
    {
        lastSpawnedPlatform.transform.SetParent(roomGridTransform, false);
    }

    GameObject nextRoomPrefab;
    Vector3 nextRoomPosition = new Vector3(0f, lastSpawnedPlatform.transform.position.y + fixedRoomOffset, 0f);

    if (!bossRoomDoorSpawned && roomCount >= 10)
    {
        // Spawn boss door and stop further spawning
        nextRoomPrefab = bossDoorPrefab;
        bossRoomDoorSpawned = true;
        stopSpawning = true; // Prevent further room spawning until the boss is defeated
    }
    else
    {
        // Continue spawning random rooms if boss door has already spawned or not yet required
        nextRoomPrefab = GetRandomRoomPrefab();
    }

    // Spawn the determined next room
    SpawnRoomAt(nextRoomPosition, false, nextRoomPrefab);
}





    private void SpawnRoomAt(Vector3 position, bool isTutorial, GameObject roomPrefab)
    {
        GameObject roomInstance = Instantiate(roomPrefab, position, Quaternion.identity);

        // Adjust position for tutorial or non-tutorial rooms
        if (isTutorial)
        {
            roomInstance.transform.position = new Vector3(0f, position.y, 0f);
        }
        else
        {
            float verticalAdjustment = position.y + fixedRoomOffset;
            roomInstance.transform.position = new Vector3(position.x, verticalAdjustment, position.z);
        }

        roomInstance.SetActive(true);

        if (roomGridTransform != null)
        {
            roomInstance.transform.SetParent(roomGridTransform, false);
        }

        if (!isTutorial)
        {
            roomCount++;
        }
        currentRoom = roomInstance;
        platformTilemap = currentRoom.GetComponentInChildren<Tilemap>();

        if (platformTilemap == null)
        {
            Debug.LogError("No Tilemap found in the spawned room.");
        }

        // Find or create the "Enemies" and "Spike Grid" containers
        GameObject enemiesContainer = GameObject.Find("Enemies") ?? new GameObject("Enemies");
        Transform spikeGrid = enemiesContainer.transform.Find("Spike Grid");
        if (spikeGrid == null)
        {
            spikeGrid = (new GameObject("Spike Grid")).transform;
            spikeGrid.SetParent(enemiesContainer.transform, false);
        }

        // Move Room Enemies to the correct containers
        Transform roomEnemies = roomInstance.transform.Find("Room Enemies");
        if (roomEnemies != null)
        {
            // DON'T CHANGE THE TWO FOR LOOPS!!!
            /*
             * if we use something like:
             * foreach (Transform child in roomEnemies)
                   child.SetParent(enemiesContainer.transform, true);
             * as the child is moved out of roomEnemies, the length of roomEnemies decreases and the loop
             * will be broken, with a result of only moving half of the children out of roomEnemies and 
             * remaining the other half.
             * 
             */
            List<Transform> enemies = new List<Transform>();
            for (int i = 0; i < roomEnemies.childCount; i++)
            {
                Transform child = roomEnemies.GetChild(i);
                enemies.Add(child);
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].name == "Spike" || enemies[i].GetComponent<Spike>() != null)
                {
                    enemies[i].SetParent(spikeGrid, true);
                }
                else
                {
                    enemies[i].SetParent(enemiesContainer.transform, true);
                }
            }
        }
    }



    private Vector3 FindHighestPlatformPosition()
    {
        if (platformTilemap == null)
        {
            Debug.LogError("Platform Tilemap is not set.");
            return Vector3.zero;
        }

        Vector3Int highestTilePos = new Vector3Int(0, int.MinValue, 0);
        BoundsInt bounds = platformTilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int localPlace = new Vector3Int(x, y, (int)platformTilemap.transform.position.z);
                if (platformTilemap.HasTile(localPlace))
                {
                    if (localPlace.y > highestTilePos.y)
                    {
                        highestTilePos = localPlace;
                    }
                }
            }
        }

        Vector3 highestWorldPos = platformTilemap.CellToWorld(highestTilePos);

        // Debug statement to log the highest platform position
        Debug.Log($"Highest platform position in current room: {highestWorldPos}");

        return highestWorldPos;
    }


    public void ResetRoomCount()
    {
        roomCount = 0;
        ToggleRoomSpawning(true); // Assuming this method exists as discussed earlier
    }

    private void OnBossKilled(BossKilledEvent e)
    {
        // Reset room spawning and boss door flags after the boss is killed
        stopSpawning = false;
        bossRoomDoorSpawned = false;

        // Spawn the after boss room prefab
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 spawnPosition = new Vector3(0f, playerPosition.y - 8f, playerPosition.z);
            GameObject newRoom = Instantiate(afterBossPrefab, spawnPosition, Quaternion.identity);
            if (roomGridTransform != null)
            {
                newRoom.transform.SetParent(roomGridTransform, false);
            }
            currentRoom = newRoom;
            platformTilemap = currentRoom.GetComponentInChildren<Tilemap>();
        }
        else
        {
            Debug.LogError("Player not found. Make sure your player GameObject is tagged correctly.");
        }
    }




}


public class BossKilledEvent
{
    // Additional fields can be added here if needed
}