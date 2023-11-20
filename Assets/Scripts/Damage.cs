using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            Spike spikeComponent = sender.GetComponent<Spike>();

            // Check for Spike component
            if (spikeComponent != null ||
                (sender.CompareTag("Enemy") && gameObject.transform.position.y > sender.transform.position.y + sender.transform.lossyScale.y / 2))
            {
                ApplyDamageToPlayer(gameObject);
            }
            else
            {
                if (gameObject.GetComponent<Player>().IsInvincible)
                    return;
                gameObject.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(Mathf.Round((amount * (gameObject.transform.position.y / 100)) + amount));
                gameObject.GetComponent<KnockBack>().PlayFeedback(sender);
                gameObject.GetComponent<Animator>().SetBool("Hit", true);
                StartCoroutine(SetAnimation(gameObject.GetComponent<Animator>()));
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
            Spike spikeComponent = sender.GetComponent<Spike>();

            // Check for Spike component
            if (spikeComponent != null ||
                (sender.CompareTag("Enemy") && gameObject.transform.position.y > sender.transform.position.y + sender.transform.lossyScale.y / 2))
            {
                ApplyDamageToPlayer(gameObject);
            }
            else
            {
                if (gameObject.GetComponent<Player>().IsInvincible)
                    return;
                gameObject.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(amount);
                gameObject.GetComponent<KnockBack>().PlayFeedback(sender);
                gameObject.GetComponent<Animator>().SetBool("Hit", true);
                StartCoroutine(SetAnimation(gameObject.GetComponent<Animator>()));
                if (gameObject.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
                    gameObject.GetComponent<Player>().Dead();
            }
        }
    }

    private void ApplyDamageToPlayer(GameObject player)
    {
        if (player.GetComponent<Player>().IsInvincible)
            return;
        player.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(amount);
        player.GetComponent<KnockBack>().PlayFeedback(this.gameObject);
        player.GetComponent<Animator>().SetBool("Hit", true);
        StartCoroutine(SetAnimation(player.GetComponent<Animator>()));
        if (player.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
            player.GetComponent<Player>().Dead();
    }

    private IEnumerator SetAnimation(Animator animator)
    {
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("Hit", false);
    }
}
