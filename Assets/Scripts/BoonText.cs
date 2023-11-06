using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoonText : MonoBehaviour
{
    private float timer;
    public Text updateText;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Time.time > timer)
        {
            updateText.gameObject.SetActive(false);
        }
    }

    public void getBoon(string input)
    {
        updateText.text = input + " has been increased";
        timer  = Time.time + 3;
        updateText.gameObject.SetActive(true);
    }
}
