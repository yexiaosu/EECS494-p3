using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class timer : MonoBehaviour
{
    private float startTime;

    void Start()
    {
        // Record the start time when the script is initialized
        startTime = Time.time;
    }

    void Update()
    {
        // Calculate the elapsed time
        float elapsedTime = Time.time - startTime;

        // Format the time as minutes and seconds
        string minutes = ((int)elapsedTime / 60).ToString("00");
        string seconds = (elapsedTime % 60).ToString("00");

        // Get the Text component of the GameObject this script is attached to
        Text timerText = GetComponent<Text>();

        // Update the UI text with the formatted time
        if (timerText != null)
        {
            timerText.text = "Timer: " + minutes + ":" + seconds;
        }
        else
        {
            Debug.LogError("TimerScript is missing Text component!");
        }
    }
}
