using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsManager : MonoBehaviour
{
    // platforms are spawn in a range, and the platforms out of the range will be destroyed
    // the range is the same as the area covered by the sky background
    public float offset;
    public GameObject platformPrefab;

    private Subscription<RepeatEvent> repeatEventSubscription;

    void Start()
    {
        repeatEventSubscription = EventBus.Subscribe<RepeatEvent>(_OnRepeat);
    }

    private void _OnRepeat(RepeatEvent e)
    {
        Vector3 initPos = e.initPos;
        Vector3 currPos = e.currPos;
        // destroy old platforms
        foreach (Transform child in transform)
        {
            if (child.position.y > initPos.y - offset / 2 && child.position.y < initPos.y + offset / 2)
            {
                Destroy(child.gameObject);
            }
        }
        // spawn new platforms
        Vector3 platformPos = new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(1.0f, 1.5f) + currPos.y - offset / 2, 0);
        int dir = 1;
        while (platformPos.y < currPos.y + offset / 2)
        {
            GameObject platform = Instantiate(platformPrefab, platformPos, Quaternion.identity);
            platform.transform.parent = transform;
            float yOffset = Random.Range(3.5f, 4.2f);
            platformPos.y = platformPos.y + yOffset;
            float xOffset = Random.Range(5.0f, 10.0f - yOffset);
            if (platformPos.x + xOffset * dir > 7.5f)
                dir = -1;
            else if (platformPos.x + xOffset * dir < -7.5f)
                dir = 1;
            platformPos.x = platformPos.x + xOffset * dir;
        }
    }
}
