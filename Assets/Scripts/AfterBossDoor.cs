using UnityEngine;

public class AfterBossDoor : MonoBehaviour
{
    public GameObject afterBossPlatformPrefab; // Assign in Inspector
    private RoomManager roomManager;

    private void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer();
            SpawnAfterBossPlatform();
            ResetRoomManager();
        }
    }

    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(0, player.transform.position.y + 15f, 0);
        }
    }
    private void SpawnAfterBossPlatform()
    {
        GameObject roomGridObject = GameObject.Find("Room Grid");
        if (afterBossPlatformPrefab != null && roomGridObject != null)
        {
            Transform roomGridTransform = roomGridObject.transform;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject afterBossPlatform = Instantiate(afterBossPlatformPrefab, new Vector3(0, player.transform.position.y - 10f, 0), Quaternion.identity);
            afterBossPlatform.transform.SetParent(roomGridTransform, false); // Set the parent to Room Grid
        }
        else
        {
            Debug.LogError("After Boss Platform Prefab or Room Grid is not set.");
        }
    }


    private void ResetRoomManager()
    {
        if (roomManager != null)
        {
            roomManager.ResetRoomCount(); // Reset the room count and enable room spawning
        }
    }
}
