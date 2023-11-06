using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 500;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<KnockBack>() != null)
            collision.GetComponent<KnockBack>().PlayFeedback(gameObject);
        if (collision.GetComponent<HealthSystemForDummies>() != null)
        {
            HealthSystemForDummies health = collision.GetComponent<HealthSystemForDummies>();
            health.AddToCurrentHealth(-damage);
            if (health.CurrentHealth <= 0)
            {
                collision.GetComponent<Enemy>().Dead();
            }
        }
    }
    public void increaseDamage(int damageInc)
    {
        damage += damageInc;
    }

}

