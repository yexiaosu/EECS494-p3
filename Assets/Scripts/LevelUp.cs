using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using TMPro;

public class LevelUp : MonoBehaviour
{
    public Text UIManagerText;
    public GameObject LevelUpPanel;
    public bool hasDisplayed = false;
    public List<BigBoon> bigBoons = new List<BigBoon> { new RangedProjectiles() };

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
            List<int> randomPos = RandomPick(bigBoons.Count);
            int i = 0;
            foreach (Transform child in LevelUpPanel.transform)
            {
                if (child.name != "Boon")
                    continue;
                if (i >= randomPos.Count)
                {
                    child.gameObject.SetActive(false);
                    continue;
                }
                child.gameObject.SetActive(true);
                child.gameObject.GetComponent<LevelUpBoon>().BigBoon = bigBoons[randomPos[i]];
                child.GetChild(0).gameObject.GetComponent<TMP_Text>().text = bigBoons[randomPos[i]].Name + ": " + bigBoons[randomPos[i]].Description;
                i++;
            }
            LevelUpPanel.SetActive(true);
        }
    }

    List<int> RandomPick(int arrayLen)
    {
        List<int> res = new List<int>();
        if (arrayLen < 3)
        {
            for (int i = 0; i < arrayLen; i++)
                res.Add(i);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                int rand = Random.Range(0, arrayLen);
                while (res.Contains(rand))
                    rand = Random.Range(0, arrayLen);
                res.Add(rand);
            }
        }
        return res;
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
