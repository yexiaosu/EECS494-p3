using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthRegen : MonoBehaviour
{
    private float playerHealth;
    private float time;
    private float healthDelay;
    private float damageDelay;
    private bool isDealying = false;
    private HealthSystemForDummies healthSystem;


    // Start is called before the first frame update
    void Start()
    {
       healthSystem = GetComponent<HealthSystemForDummies>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth > healthSystem.CurrentHealth)
        {
            isDealying = true;
            damageDelay = Time.time + 20;
        }
        playerHealth = healthSystem.CurrentHealth;
        if(isDealying && Time.time > damageDelay)
        {
            isDealying = false;
        }
        if (!isDealying && Time.time > healthDelay)
        {
            healthSystem.AddToCurrentHealth(5);
            healthDelay = Time.time + 1;
        }
    }
}
