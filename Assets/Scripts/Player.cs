using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int attack = 500;
    public bool IsInvincible = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void Dead()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<MessageDisplay>().DisplayMessage("death");
    }

    public void IncreaseAttack(int damageInc)
    {
        attack += damageInc;
    }
}
