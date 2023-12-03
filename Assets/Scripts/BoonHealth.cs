using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class BoonHealth : MonoBehaviour
{
    private float speedBoost = 1f;  // The additional speed the player gains
    private float jumpBoost = .5f;   // The additional jump force the player gains
    //public GameObject Canvas;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object has the PlayerMovement script
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player)
        {
            BoonText bt = collision.gameObject.GetComponent<BoonText>();
            GameObject attackArea = collision.transform.GetChild(0).gameObject;
            AttackArea atk = attackArea.GetComponent<AttackArea>();
            HealthSystemForDummies playerHP = collision.gameObject.GetComponent<HealthSystemForDummies>();
            //maybe add attack size and speed change
            //add hp change
            // make random
            // Increase player's speed and jump
            //int roll = Random.Range(4, 6);
            int roll = 5; // Only one outcome
            if(roll == 1)
            {
                player.changeSpeed(player.GetMoveSpeed() + speedBoost);
                bt.getBoon("Speed");
            }
            else if(roll == 2)
            
            {
                collision.gameObject.GetComponent<Player>().IncreaseAttack(250);
                bt.getBoon("Attack Damage");
                /*
                player.changeJump(player.GetJumpForce() + jumpBoost);
                bt.getBoon("Jump");
                */
            }
          
            else if (roll == 3) 
            {
                attackArea.transform.localScale = attackArea.transform.localScale * 1.2f;
                bt.getBoon("Attack Area");
            }
            else if(roll ==  4)
            {
                playerHP.AddToCurrentHealth(500);
                bt.getBoon("Current Player Health");
            }
            else if(roll == 5)
            {
                playerHP.AddToMaximumHealth(250);
                playerHP.AddToCurrentHealth(250);
                bt.getBoon("Max Player Health");
            }
            else if(roll == 6)
            {
                
            }

            
            

            
            
            // Destroy the boon
            Destroy(gameObject);
        }
    }
}
