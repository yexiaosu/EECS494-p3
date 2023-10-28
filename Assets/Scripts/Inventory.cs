using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Keeps track of num  and types of powerups
    private int numJumpUps = 0;
    private int numSpeedUps = 0;
        //add more powerups

    //needed to grab player info
    private PlayerMovement pm;

    //on start grabs player info
    private void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }

    //used to update the values of player info
    private void updateJump()
    {
        pm.changeJump(numJumpUps + 5);
    }
    private void updateSpeed()
    {
        pm.changeSpeed(numSpeedUps + 5);
    }


    //Allows you to add/remove jumps/speed from  other scripts
    public void addJump()
    {
        numJumpUps++;
        updateJump();
    }
    public void addSpeed()
    {
        numSpeedUps++;
        updateSpeed();
    }
    public void removeJump()
    {
        numJumpUps--;
        updateJump();
    }
    public void removeSpeed()
    {
        numSpeedUps--;
        updateSpeed();
    }
}
