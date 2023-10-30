using UnityEngine;
using UnityEngine.UI;

public class CongratulationsMessage : MonoBehaviour
{
    public Text UIManagerText; 
    private UIManager UIManagerObject;
    public Text messageText; // The UI text component to display the message

    private bool hasDisplayedMessage = false;

    void Start()
    {
        // Assuming the LevelUpManager script is attached to the player object
        UIManagerObject = UIManagerText.GetComponent<UIManager>();

        // Initially hide the message
        messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!hasDisplayedMessage && UIManagerObject.GetScore() >= 100)
        {
            Debug.Log("past if statement");
            DisplayMessage();
            hasDisplayedMessage = true;
        }
    }


    void DisplayMessage()
    {
        messageText.gameObject.SetActive(true);
    }
}
