using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<KnockBack>() != null)
            collision.GetComponent<KnockBack>().PlayFeedback(gameObject);

        if (collision.GetComponent<HealthSystemForDummies>() != null)
        {
            HealthSystemForDummies health = collision.GetComponent<HealthSystemForDummies>();
            int damage = GameObject.Find("Player").GetComponent<Player>().attack;

            // Check if the collided object is an Enemy
            if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<Enemy>().GetMeeleHit();
                health.AddToCurrentHealth(-damage);
                if (health.CurrentHealth <= 0)
                {
                    collision.GetComponent<Enemy>().Dead();
                }
            }
            // Check if the collided object is a BossEnemy
            else if (collision.GetComponent<BossAI>() != null)
            {
                collision.GetComponent<BossAI>().GetMeleeHit();
                health.AddToCurrentHealth(-damage);
                if (health.CurrentHealth <= 0)
                {
                    collision.GetComponent<BossAI>().Dead();
                }
            }
        }
    }
}
