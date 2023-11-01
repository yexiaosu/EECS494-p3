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
            gameObject.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(amount);
            gameObject.GetComponent<KnockBack>().PlayFeedback(sender);
            if (gameObject.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
                gameObject.GetComponent<Player>().Dead();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
        GameObject sender = this.gameObject;

        if (gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(amount);
            gameObject.GetComponent<KnockBack>().PlayFeedback(sender);
            if (gameObject.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
                gameObject.GetComponent<Player>().Dead();
        }
    }
}
