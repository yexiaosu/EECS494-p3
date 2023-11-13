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

    void Start()
    {
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
        Vector3 highestPlatformPosition = FindHighestPlatformPosition();
        Vector3 transitionPosition = new Vector3(0f, highestPlatformPosition.y - 8f, 0f);

        lastSpawnedPlatform = Instantiate(transitionPlatformPrefab, transitionPosition, Quaternion.identity);
        if (roomGridTransform != null)
        {
            lastSpawnedPlatform.transform.SetParent(roomGridTransform, false);
        }

        GameObject nextRoomPrefab = roomCount < 10 ? GetRandomRoomPrefab() : bossDoorPrefab;
        Vector3 nextRoomPosition = new Vector3(0f, lastSpawnedPlatform.transform.position.y + fixedRoomOffset, 0f);
        SpawnRoomAt(nextRoomPosition, false, nextRoomPrefab);
    }


    private void SpawnRoomAt(Vector3 position, bool isTutorial, GameObject roomPrefab)
    {
        GameObject roomInstance = Instantiate(roomPrefab, new Vector3(0f, -1000f, 0f), Quaternion.identity);

        if (!isTutorial)
        {
            float verticalAdjustment = position.y + fixedRoomOffset;
            roomInstance.transform.position = new Vector3(position.x, verticalAdjustment, position.z);
        }
        else
        {
            roomInstance.transform.position = position;
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
                Vector3Int localPlace = (new Vector3Int(x, y, (int)platformTilemap.transform.position.z));
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
        return highestWorldPos;
    }

    public void ResetRoomCount()
    {
        roomCount = 0;
        ToggleRoomSpawning(true); // Assuming this method exists as discussed earlier
    }

}
