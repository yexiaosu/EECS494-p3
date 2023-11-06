using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Bullet;
    public bool ProjectileEnabled = false;
    public float ProjectileCd = 2.0f;

    private GameObject attackAreaRight = default;
    private GameObject attackAreaLeft = default;
    private bool attackingRight = false;
    private bool attackingLeft = false;
    private float timeToAttack = .25f;
    private float timerLeft = 0f;
    private float timerRight = 0f;
    private float timerProjectile = 0f;
    // Start is called before the first frame update
    void Start()
    {
        attackAreaRight = transform.GetChild(0).gameObject;
        attackAreaLeft = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (ProjectileEnabled)
        {
            timerProjectile += Time.deltaTime;
            if (timerProjectile > ProjectileCd)
            {
                Projectile();
                timerProjectile = 0;
            }
        }
        if(Input.GetKeyDown(KeyCode.Mouse1)) 
        {
            AttackRight();
        }
        if(attackingRight)
        {
            timerRight += Time.deltaTime;
            if(timerRight >= timeToAttack ) 
            { 
                timerRight = 0f;
                attackingRight = false;
                attackAreaRight.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttackLeft();
        }
        if (attackingLeft)
        {
            timerLeft += Time.deltaTime;
            if (timerLeft >= timeToAttack)
            {
                timerLeft = 0f;
                attackingLeft = false;
                attackAreaLeft.SetActive(false);
            }
        }
    }

    private void AttackRight()
    {
        attackingRight = true;
        attackAreaRight.SetActive(attackingRight);
    }

    private void AttackLeft()
    {
        attackingLeft = true;
        attackAreaLeft.SetActive(attackingLeft);
    }

    private void Projectile()
    {
        GameObject projectile = Instantiate(Bullet, transform.position, Quaternion.identity);
    }
}
