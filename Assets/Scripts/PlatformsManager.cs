using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsManager : MonoBehaviour
{
    public float offset;
    public GameObject platformPrefab;
    public GameObject boonPrefab;

    private Subscription<RepeatEvent> repeatEventSubscription;
    private HashSet<int> touchedPlatforms = new HashSet<int>();
    private int platformIDCounter = 0;

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

        // Spawn new platforms
        Vector3 platformPos = new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(1.0f, 1.5f) + currPos.y - offset / 2, 0);
        int dir = 1;
        while (platformPos.y < currPos.y + offset / 2)
        {
            GameObject platform = Instantiate(platformPrefab, platformPos, Quaternion.identity);
            platform.transform.parent = transform;
            platform.GetComponent<Platform>().PlatformID = platformIDCounter++;

            // 2.5% chance to spawn a boon on the platform
            if (Random.value <= 0.025f)

            {
                Vector3 boonPos = new Vector3(platformPos.x, platformPos.y + platform.GetComponent<Renderer>().bounds.size.y, platformPos.z);
                Instantiate(boonPrefab, boonPos, Quaternion.identity);
            }

            float yOffset = Random.Range(3.5f, 4.2f);
            platformPos.y += yOffset;
            float xOffset = Random.Range(5.0f, 8.0f - yOffset);
            if (platformPos.x + xOffset * dir > 7.5f)
                dir = -1;
            else if (platformPos.x + xOffset * dir < -7.5f)
                dir = 1;
            platformPos.x += xOffset * dir;
        }
    }
}

public struct PlayerTouchedPlatformEvent
{
    public int PlatformID;

    public PlayerTouchedPlatformEvent(int id)
    {
        PlatformID = id;
    }
}
