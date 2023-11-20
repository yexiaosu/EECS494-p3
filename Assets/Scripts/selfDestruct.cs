using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestruct : MonoBehaviour
{
    public float destructionDelay = 5f;
    private float timer;

    void Start()
    {
        timer = Time.time + destructionDelay;
    }

    private void Update()
    {
        if(timer < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
