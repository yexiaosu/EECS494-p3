using System.Collections.Generic;
using UnityEngine;

public class ItemShopManager : MonoBehaviour
{
    public List<GameObject> itemPrefabs; // The pool of item prefabs
    public Transform[] spawnPoints; // Predetermined spawn points for the items
    public Transform cameraPosition; // The position to move the camera to

    private Subscription<EnterItemShopEvent> enterShopSubscription;

    private void Awake()
    {
        // Subscribe to the enter item shop event
        enterShopSubscription = EventBus.Subscribe<EnterItemShopEvent>(OnEnterItemShop);
    }

    private void OnDestroy()
    {
        // Unsubscribe when the object is destroyed
        EventBus.Unsubscribe<EnterItemShopEvent>(enterShopSubscription);
    }

    private void OnEnterItemShop(EnterItemShopEvent evt)
    {
        MoveCamera();
        SpawnRandomItems();
    }

    private void MoveCamera()
    {
        Camera.main.transform.position = cameraPosition.position;
    }

    private void SpawnRandomItems()
    {
        // Shuffle the list of item prefabs to randomize what gets spawned
        System.Random rng = new System.Random();
        int n = itemPrefabs.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = itemPrefabs[k];
            itemPrefabs[k] = itemPrefabs[n];
            itemPrefabs[n] = value;
        }

        // Spawn the items at the spawn points
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i < itemPrefabs.Count)
            {
                Instantiate(itemPrefabs[i], spawnPoints[i].position, Quaternion.identity);
            }
        }
    }
}

public class EnterItemShopEvent
{
    public Vector3 playerPositionBeforeEntering;
    public Vector3 cameraPositionBeforeEntering;

    public EnterItemShopEvent(Vector3 playerPos, Vector3 cameraPos)
    {
        playerPositionBeforeEntering = playerPos;
        cameraPositionBeforeEntering = cameraPos;
    }
}


public class ExitItemShopEvent
{

}