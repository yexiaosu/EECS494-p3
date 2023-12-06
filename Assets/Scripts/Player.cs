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
    public void Dead(string name)
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<MessageDisplay>().DisplayMessage("death", name);
        EventBus.Publish<PauseEvent>(new PauseEvent());
    }

    public void IncreaseAttack(int damageInc)
    {
        attack += damageInc;
    }
}
