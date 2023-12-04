using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fallDamage : MonoBehaviour
{
    public float fallHeightThreshold = 15f;
    public float damageMultiplier = 10f;

    private bool isFalling = false;
    private Vector3 fallStartPosition;
    private string fallStartSceneName;

    private HealthSystemForDummies healthSystem;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystemForDummies>();
    }

    void Update()
    {
        // Check if the player is falling
        if (IsFalling())
        {
            if (!isFalling)
            {
                fallStartPosition = transform.position;
                isFalling = true;
                // Store the current scene name
                fallStartSceneName = SceneManager.GetActiveScene().name;
            }
        }
        else
        {
            if (isFalling)
            {
                // Check if the scene has changed since the player started falling
                if (SceneManager.GetActiveScene().name != fallStartSceneName)
                {
                    // The scene has changed, don't apply fall damage
                    isFalling = false;
                    return;
                }
                // Player has stopped falling
                float fallDistance = fallStartPosition.y - transform.position.y;

                // Apply fall damage if the fall distance exceeds the threshold
                if (fallDistance > fallHeightThreshold)
                {
                    int damageAmount = Mathf.RoundToInt((fallDistance - fallHeightThreshold) * damageMultiplier * fallDistance);
                    if (damageAmount > 0)
                    {
                        ApplyDamage(damageAmount);
                    }
                    
                }

                isFalling = false;
            }
        }
    }

    bool IsFalling()
    {
        // Check if the player is falling based on the y-velocity
        return GetComponent<Rigidbody2D>().velocity.y < 0f;
    }

    void ApplyDamage(int damageAmount)
    {
        healthSystem.AddToCurrentHealth(-damageAmount);
        if (this.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
            this.GetComponent<Player>().Dead("Gravity");
    }
}
