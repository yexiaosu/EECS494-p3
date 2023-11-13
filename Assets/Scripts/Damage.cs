using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int amount = -50;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;
        GameObject sender = this.gameObject;

        if (gameObject.CompareTag("Player"))
        {
            // player jump on the enemy's head
            if (sender.CompareTag("Enemy") && gameObject.transform.position.y > sender.transform.position.y + sender.transform.lossyScale.y / 2)
            {
                HealthSystemForDummies health = sender.GetComponent<HealthSystemForDummies>();
                int damage = GameObject.Find("Player").GetComponent<Player>().attack;
                health.AddToCurrentHealth(-damage);
                if (health.CurrentHealth <= 0)
                {
                    sender.GetComponent<Enemy>().Dead();
                }
                gameObject.GetComponent<KnockBack>().PlayFeedback(sender);
            }
            else
            {
                gameObject.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(amount);
                gameObject.GetComponent<KnockBack>().PlayFeedback(sender);
                if (gameObject.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
                    gameObject.GetComponent<Player>().Dead();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
        GameObject sender = this.gameObject;

        if (gameObject.CompareTag("Player"))
        {
            // player jump on the enemy's head
            if (sender.CompareTag("Enemy") && gameObject.transform.position.y > sender.transform.position.y + sender.transform.lossyScale.y / 2)
            {
                HealthSystemForDummies health = sender.GetComponent<HealthSystemForDummies>();
                int damage = GameObject.Find("Player").GetComponent<Player>().attack;
                health.AddToCurrentHealth(-damage);
                if (health.CurrentHealth <= 0)
                {
                    sender.GetComponent<Enemy>().Dead();
                }
                gameObject.GetComponent<KnockBack>().PlayFeedback(sender);
            }
            else
            {
                gameObject.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(amount);
                gameObject.GetComponent<KnockBack>().PlayFeedback(sender);
                if (gameObject.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
                    gameObject.GetComponent<Player>().Dead();
            }
        }
    }
}
