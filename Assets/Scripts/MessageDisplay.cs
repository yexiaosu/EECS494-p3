using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MessageDisplay: MonoBehaviour
{
    public Text UIManagerText;
    private UIManager UIManagerObject;
    public GameObject panel; // The UI text component to display the message
    public GameObject death;
    public GameObject killedby;

    private bool hasDisplayedMessage = false;

    void Start()
    {
        // Assuming the LevelUpManager script is attached to the player object
        GameObject UIManagerText = GameObject.Find("UIText");
        UIManagerObject = UIManagerText.GetComponent<UIManager>();

        // Initially hide the message
        panel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (hasDisplayedMessage && Input.GetKeyDown(KeyCode.Space))
        {
            // Reload the current scene
            Destroy(GameObject.Find("Player").gameObject);
            Destroy(GameObject.Find("CanvasMain").gameObject);
            SceneManager.LoadScene("Start");
        }
    }


    public void DisplayMessage(string type, string targetName)
    {
        panel.SetActive(true);
        death.SetActive(false);
        if (type == "death")
        {
            death.SetActive(true);
            death.GetComponent<Text>().text = "You get score of: " + UIManagerObject.GetScore().ToString();
            killedby.GetComponent<Text>().text = targetName == "" ? "Killed by... Unknown damage" : "Killed by... " + targetName;
        }
        hasDisplayedMessage = true;
    }
}
