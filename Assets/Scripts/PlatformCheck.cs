using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCheck : MonoBehaviour
{
    private bool eventSent = false;
    private Transform playerTransform; // Store the transform for easier access

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
        {
            playerTransform = playerObject.transform;
        }
    }

    private void Update()
    {
        if (playerTransform && !eventSent && playerTransform.position.y > transform.position.y + (this.GetComponent<Renderer>().bounds.size.y / 2))
        {
            eventSent = true; // mark the event as sent
        }
    }
}
