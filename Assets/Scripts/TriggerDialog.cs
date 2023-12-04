using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialog : MonoBehaviour
{
    public GameObject InteractHint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("PlayerStory").GetComponent<PlayerSimpleMovement>().AbleToTriggerDialog = true;
        InteractHint.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject.Find("PlayerStory").GetComponent<PlayerSimpleMovement>().AbleToTriggerDialog = false;
        InteractHint.SetActive(false);
    }
}
