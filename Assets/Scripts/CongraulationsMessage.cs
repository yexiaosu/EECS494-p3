using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CongratulationsMessage : MonoBehaviour
{
    public Text UIManagerText; 
    private UIManager UIManagerObject;
    public GameObject congratulationsPanel; // The UI text component to display the message

    private bool hasDisplayedMessage = false;

    void Start()
    {
        // Assuming the LevelUpManager script is attached to the player object
        UIManagerObject = UIManagerText.GetComponent<UIManager>();

        // Initially hide the message
        congratulationsPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!hasDisplayedMessage && UIManagerObject.GetScore() >= 100)
        {
            Debug.Log("past if statement");
            DisplayMessage();
            hasDisplayedMessage = true;
        }

        if (hasDisplayedMessage && Input.GetKeyDown(KeyCode.Space))
        {
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    void DisplayMessage()
    {
        congratulationsPanel.SetActive(true);

    }
}
