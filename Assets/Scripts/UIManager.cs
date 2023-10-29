using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    public Text scoreAndLevelText;  

    private int score = 0;
    private int level = 0;

    private void Start()
    {
        UpdateScoreAndLevelText();

        // Subscribe to the PlayerTouchedPlatformEvent
        EventBus.Subscribe<PlayerTouchedPlatformEvent>(OnPlayerTouchedPlatform);
    }

    private void OnPlayerTouchedPlatform(PlayerTouchedPlatformEvent evt)
    {
        score += 1;
        level += 1;

        // Update the text display
        UpdateScoreAndLevelText();
    }

    private void UpdateScoreAndLevelText()
    {
        scoreAndLevelText.text = $"Score: {score}\nLevel: {level}";
    }

    // Remember to unsubscribe when the object is destroyed

}
