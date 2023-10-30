using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreAndLevelText;
    public Transform playerTransform; 
    private float highestYValue = 0f;

    private void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in LevelUpManager!");
            return;
        }

        UpdateScoreAndLevelText();

        EventBus.Subscribe<PlayerTouchedPlatformEvent>(OnPlayerTouchedPlatform);
    }

    private void Update()
    {
        // Check the current Y value of the player
        float currentY = playerTransform.position.y;

        if (currentY > highestYValue)
        {
            highestYValue = currentY;
            UpdateScoreAndLevelText();
        }
    }

    private void OnPlayerTouchedPlatform(PlayerTouchedPlatformEvent evt)
    {
    }

    private void UpdateScoreAndLevelText()
    {
        int displayScore = Mathf.Max(0, Mathf.FloorToInt(highestYValue));
        scoreAndLevelText.text = $"Score: {displayScore - 9}";
    }

    // Public getter for the score
    public int GetScore()
    {
        return Mathf.Max(0, Mathf.FloorToInt(highestYValue));
    }
}
