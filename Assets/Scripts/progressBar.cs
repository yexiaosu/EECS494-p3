using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class progressBar : MonoBehaviour
{
    private Transform playerTransform; // Reference to the player's transform
    public string playerTag = "Player"; // Tag of the player GameObject

    void Start()
    {
        // Automatically find the player's transform based on the specified tag
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the specified tag.");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float playerHeight = playerTransform.position.y;
            if(playerHeight % 150 < 15)
            {
                playerHeight = playerHeight % 160;
            }
            else
            {
                playerHeight = playerHeight % 150;
            }

            playerHeight = playerHeight / 150;
            playerHeight *= .71f;
            transform.localScale = new Vector3(.69f, playerHeight, 1);
        }
        else
        {
            Debug.LogError("Player transform not assigned!");
        }
    }
}
