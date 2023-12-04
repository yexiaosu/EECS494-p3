using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialog : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("PlayerStory").GetComponent<PlayerSimpleMovement>().AbleToTriggerDialog = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject.Find("PlayerStory").GetComponent<PlayerSimpleMovement>().AbleToTriggerDialog = false;
    }
}
