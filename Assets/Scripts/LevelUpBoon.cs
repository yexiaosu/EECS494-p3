using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpBoon : MonoBehaviour
{
    public GameObject LevelUpPanel;

    public void ChooseBoon()
    {
        EventBus.Publish<ResumeEvent>(new ResumeEvent());
        LevelUpPanel.SetActive(false);
        GameObject.Find("Player").GetComponent<LevelUp>().hasDisplayed = false;
    }
}
