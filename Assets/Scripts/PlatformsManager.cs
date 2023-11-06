using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsManager : MonoBehaviour
{
    public float offset;
    public GameObject platformPrefab;
    public GameObject boonPrefab;
    public GameObject itemShopEntryPrefab; // Prefab for the item shop entry platform
    public GameObject enterPromptUI;

    private Subscription<RepeatEvent> repeatEventSubscription;
    private HashSet<int> touchedPlatforms = new HashSet<int>();
    private int platformIDCounter = 0;
    private int nextShopSpawnScore = 200; // The score at which the next shop will spawn

    void Start()
    {
        repeatEventSubscription = EventBus.Subscribe<RepeatEvent>(_OnRepeat);
    }

    private void _OnRepeat(RepeatEvent e)
    {
        Vector3 initPos = e.initPos;
        Vector3 currPos = e.currPos;

        // Destroy old platforms
        foreach (Transform child in transform)
        {
            if (child.position.y > initPos.y - offset / 2 && child.position.y < initPos.y + offset / 2)
            {
                Destroy(child.gameObject);
            }
        }

        // Check if it's time to spawn the item shop entry platform
        int playerScore = UIManager.Instance.GetScore();
        if (playerScore >= nextShopSpawnScore)
        {
            SpawnItemShopEntry(currPos.y);
            nextShopSpawnScore += 200; // Increment to the next threshold
        }
        else
        {
            // Continue spawning platforms as normal
            SpawnPlatforms(currPos);
        }
    }

    private void SpawnItemShopEntry(float currentPosY)
    {
        // Determine the position for the item shop entry platform
        Vector3 shopEntryPos = new Vector3(0, currentPosY + 5f, 0); // Adjust Y as needed
        GameObject shopInstance = Instantiate(itemShopEntryPrefab, shopEntryPos, Quaternion.identity);

        // Assuming your item shop prefab has the ItemShopDoor script attached to it
        ItemShopDoor doorScript = shopInstance.GetComponentInChildren<ItemShopDoor>();
        if (doorScript != null)
        {
            // Set the reference to the UI element directly
            doorScript.enterPrompt = enterPromptUI;
        }
    }

    private void SpawnPlatforms(Vector3 currPos)
    {
        // Existing logic for spawning platforms
        Vector3 platformPos = new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(1.0f, 1.5f) + currPos.y - offset / 2, 0);
        int dir = 1;
        while (platformPos.y < currPos.y + offset / 2)
        {
            GameObject platform = Instantiate(platformPrefab, platformPos, Quaternion.identity);
            platform.transform.parent = transform;
            platform.GetComponent<Platform>().PlatformID = platformIDCounter++;

            // Spawn boon on the platform with a chance
            TrySpawnBoon(platformPos);

            // Determine the next platform's position
            platformPos = CalculateNextPlatformPosition(platformPos, ref dir);
        }
    }

    private void TrySpawnBoon(Vector3 platformPos)
    {
        if (Random.value <= 0.025f)
        {
            Vector3 boonPos = new Vector3(platformPos.x, platformPos.y + platformPrefab.GetComponent<Renderer>().bounds.size.y, platformPos.z);
            Instantiate(boonPrefab, boonPos, Quaternion.identity);
        }
    }

    private Vector3 CalculateNextPlatformPosition(Vector3 platformPos, ref int dir)
    {
        float yOffset = Random.Range(3.5f, 4.2f);
        platformPos.y += yOffset;
        float xOffset = Random.Range(5.0f, 8.0f - yOffset);
        if (platformPos.x + xOffset * dir > 7.5f)
            dir = -1;
        else if (platformPos.x + xOffset * dir < -7.5f)
            dir = 1;
        platformPos.x += xOffset * dir;

        return platformPos;
    }
}
