using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<HealthSystemForDummies>() != null)
        {
            HealthSystemForDummies health = collision.GetComponent<HealthSystemForDummies>();
            health.AddToCurrentHealth(-1000);
        }
    }
}
