using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextTextAnime : MonoBehaviour
{
    private Text next;
    private float timer = 0;
    private bool hasDash = true;

    // Start is called before the first frame update
    void Start()
    {
        next = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            hasDash = !hasDash;
            if (hasDash)
                next.text = "Click_";
            else
                next.text = "Click";
            timer = 0;
        }
    }
}
