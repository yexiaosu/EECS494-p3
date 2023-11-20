using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } // Singleton pattern
    public Text scoreAndLevelText;
    private Transform playerTransform;
    private float highestYValue = 0f;
    private int nextShopSpawnThreshold = 200; // Initial threshold for spawning the item shop
    public int scoreOffset = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;

        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in UIManager!");
            return;
        }

        UpdateScoreAndLevelText();
        // If you're using an event system, you would subscribe to the event here.
    }

    private void Update()
    {
        // Check the current Y value of the player
        float currentY = playerTransform.position.y;

        if (currentY + scoreOffset > highestYValue)
        {
            highestYValue = currentY + scoreOffset;
            UpdateScoreAndLevelText();

            // Check if the player has reached a new shop spawn threshold
            if (highestYValue >= nextShopSpawnThreshold)
            {
                // Send a SpawnItemShop event
                EventBus.Publish(new SpawnItemShopEvent());

                // Set the next threshold, assuming a consistent increase by 200 each time
                nextShopSpawnThreshold += 200;
            }
        }
    }

    private void UpdateScoreAndLevelText()
    {
        int displayScore = Mathf.Max(0, Mathf.FloorToInt(highestYValue));
        scoreAndLevelText.text = $"Score: {displayScore - 9}"; // Assuming you want to start scoring from 0
    }

    // Public getter for the score
    public int GetScore()
    {
        return Mathf.Max(0, Mathf.FloorToInt(highestYValue));
    }

    public void ChangeOffset(int i)
    {
        scoreOffset += i;
    }
}

// Define the SpawnItemShopEvent
public class SpawnItemShopEvent
{
    // Add any fields or properties that you need to pass with this event.
}
