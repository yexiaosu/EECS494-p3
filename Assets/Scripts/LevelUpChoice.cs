using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChoice : MonoBehaviour
{
    public GameObject LevelUpPanel;
    public BigBoon BigBoon;
    public GameObject GetAbilityText;

    public void ChooseBoon()
    {
        LevelUp lu = GameObject.Find("Player").GetComponent<LevelUp>();
        if (BigBoon != null)
        {
            GetAbilityText.SetActive(true);
            GetAbilityText.GetComponent<Text>().text = BigBoon.CurrLevel == 0 ? BigBoon.Name + " Obtained!" : BigBoon.Name + " Level Up!";
            if (BigBoon.CurrLevel == 0)
                BigBoon.GetBoon();
            else
                BigBoon.LevelUpBoon();
            if (!BigBoon.CanLevelUp)
            {
                var itemToRemove = lu.bigBoons.SingleOrDefault(r => r.Name == BigBoon.Name);
                if (itemToRemove != null)
                    lu.bigBoons.Remove(itemToRemove);
            }
        }
        EventBus.Publish<ResumeEvent>(new ResumeEvent());
        LevelUpPanel.SetActive(false);
        GameObject.Find("Player").GetComponent<LevelUp>().LevelUpText.SetActive(false);
        lu.hasDisplayed = false;
    }
}
