using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public Text UIManagerText;
    public GameObject LevelUpPanel;
    public bool hasDisplayed = false;
    public GameObject LevelUpText;
    public GameObject BackLevelUpAnime;
    public GameObject FrontLevelUpAnime;

    public List<BigBoon> bigBoons = new List<BigBoon> { new RangedProjectiles() , new MissileAttack(), new DoubleJump(), new Dash(), new Shield(), new Stomp() };

    private UIManager UIManagerObject;
    private int lastScore = -50;
    private GameObject player;
    private bool isAnimePlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        UIManagerObject = UIManagerText.GetComponent<UIManager>();
        LevelUpPanel.SetActive(false);
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDisplayed && UIManagerObject.GetScore() - lastScore >= 50 && !isAnimePlayed && lastScore >= 0)
        {
            StartCoroutine(LevelUpAnimation());
            isAnimePlayed = true;
        }
        if (!hasDisplayed && player.GetComponent<PlayerMovement>().GetIsGrounded() && UIManagerObject.GetScore() - lastScore >= 50)
            {
            if (lastScore < 0)
                LevelUpPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Initial Boon";
            else
                LevelUpPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Level Up";
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
                child.gameObject.GetComponent<LevelUpChoice>().BigBoon = bigBoons[randomPos[i]];
                string levelPrefix = bigBoons[randomPos[i]].CanLevelUp ? "[Lv." + (bigBoons[randomPos[i]].CurrLevel + 1).ToString() + "] " : "";
                child.GetChild(0).gameObject.GetComponent<Text>().text = levelPrefix + bigBoons[randomPos[i]].Name + ": " + bigBoons[randomPos[i]].Description;
                i++;
            }
            LevelUpPanel.SetActive(true);
            isAnimePlayed = false;
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

    private IEnumerator LevelUpAnimation()
    {
        LevelUpText.SetActive(true);
        BackLevelUpAnime.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        BackLevelUpAnime.SetActive(false);
        FrontLevelUpAnime.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        FrontLevelUpAnime.SetActive(false);
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
