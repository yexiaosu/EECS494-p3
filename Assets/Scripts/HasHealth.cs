using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public int InitialMaxHealth = 1000;

    // Start is called before the first frame update
    void Start()
    {
        HealthSystemForDummies health = GetComponent<HealthSystemForDummies>();
        health.MaximumHealth = Mathf.Round(InitialMaxHealth + (InitialMaxHealth * (gameObject.transform.position.y / 100)));
        health.CurrentHealth = Mathf.Round(InitialMaxHealth + (InitialMaxHealth * (gameObject.transform.position.y / 100)));
    }
}
