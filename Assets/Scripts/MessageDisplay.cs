using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MessageDisplay: MonoBehaviour
{
    public Text UIManagerText; 
    private UIManager UIManagerObject;
    public GameObject panel; // The UI text component to display the message
    public GameObject death;

    private bool hasDisplayedMessage = false;

    void Start()
    {
        // Assuming the LevelUpManager script is attached to the player object
        UIManagerObject = UIManagerText.GetComponent<UIManager>();

        // Initially hide the message
        panel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (hasDisplayedMessage && Input.GetKeyDown(KeyCode.Space))
        {
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    public void DisplayMessage(string type)
    {
        panel.SetActive(true);
        death.SetActive(false);
        if (type == "death")
            death.SetActive(true);
        hasDisplayedMessage = true;
    }
}
