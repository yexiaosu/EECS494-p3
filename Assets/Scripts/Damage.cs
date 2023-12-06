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
            // player jump on the enemy's head
            // DON'T CHANGE HERE!!!!!
            /*
             * The logic is for damaging enemies when jumpping on their heads.
             * It's hurting ENEMIES instead of PLAYER!!!
             * The logics for hurting the player should be added to else block.
             */
            if (sender.CompareTag("Enemy") && gameObject.transform.position.y > sender.transform.position.y + gameObject.transform.lossyScale.y / 2 && spikeComponent == null)
            {
                // if the sender is not a spike
                // apply damage to enemy
                HealthSystemForDummies health = sender.GetComponent<HealthSystemForDummies>();
                if (health == null)
                    return;
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
                // if the sender is a spike
                ApplyDamageToPlayer(gameObject, sender);
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
            // player jump on the enemy's head
            // DON'T CHANGE HERE!!!!!
            /*
             * The logic is for damaging enemies when jumpping on their heads.
             * It's hurting ENEMIES instead of PLAYER!!!
             * The logics for hurting the player should be added to else block.
             */
            if (sender.CompareTag("Enemy") && gameObject.transform.position.y > sender.transform.position.y + gameObject.transform.lossyScale.y / 2 && spikeComponent == null)
            {
                // if the sender is not a spike
                // apply damage to enemy
                HealthSystemForDummies health = sender.GetComponent<HealthSystemForDummies>();
                if (health == null)
                    return;
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
                // if the sender is a spike or not jumping on head
                ApplyDamageToPlayer(gameObject, sender);
            }
        }
    }

    private void ApplyDamageToPlayer(GameObject player, GameObject sender)
    {
        if (player.GetComponent<Player>().IsInvincible)
            return;
        // scaling for damage
        player.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(Mathf.Round((amount * (gameObject.transform.position.y / 100)) + amount));
        player.GetComponent<KnockBack>().PlayFeedback(sender);
        player.GetComponent<Animator>().SetBool("Hit", true);
        StartCoroutine(SetAnimation(player.GetComponent<Animator>()));
        if (player.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
        {
            string name = sender.GetComponent<TargetName>() == null ? "" : sender.GetComponent<TargetName>().targetName;
            player.GetComponent<Player>().Dead(name);
        }
    }

    private IEnumerator SetAnimation(Animator animator)
    {
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("Hit", false);
    }
}
