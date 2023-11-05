using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public Text UIManagerText;
    public GameObject LevelUpPanel;
    public bool hasDisplayed = false;

    private UIManager UIManagerObject;
    private int lastScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        UIManagerObject = UIManagerText.GetComponent<UIManager>();
        LevelUpPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDisplayed && UIManagerObject.GetScore() - lastScore >= 100)
        {
            EventBus.Publish<PauseEvent>(new PauseEvent());
            lastScore = UIManagerObject.GetScore();
            LevelUpPanel.SetActive(true);
        }
    }
}

public class PauseEvent
{
    public PauseEvent() {  }
}

public class ResumeEvent
{
    public ResumeEvent() {  }
}
