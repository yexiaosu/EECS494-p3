using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Bullet;
    public bool ProjectileEnabled = false;
    public float ProjectileCd = 2.0f;
    public GameObject DirectionSprite;

    private GameObject meeleAttackArea = default;
    private bool meeleAttacking = false;
    private float timeToAttack = .3f;
    private float timerMeeleAttack = 0f;
    private float timerProjectile = 0f;
    // Start is called before the first frame update
    void Start()
    {
        meeleAttackArea = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse direction
        Vector3 positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // make the Z position equal to the player for a fully 2D comparison
        positionMouse.z = transform.position.z;
        Vector3 vectorAttack = (positionMouse - transform.position).normalized;
        // make direction visualization point to the mouse direction
        DirectionSprite.transform.position = transform.position + vectorAttack;
        DirectionSprite.transform.rotation = new Quaternion(0,0,0,0);
        DirectionSprite.transform.RotateAround(DirectionSprite.transform.position, new Vector3(0, 0, 1), Mathf.Atan2(vectorAttack.y, vectorAttack.x) / Mathf.PI * 180.0f);
        if (ProjectileEnabled)
        {
            timerProjectile += Time.deltaTime;
            if (timerProjectile > ProjectileCd)
            {
                Projectile();
                timerProjectile = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MeeleAttack(vectorAttack);
        }
        if (meeleAttacking)
        {
            timerMeeleAttack += Time.deltaTime;
            if (timerMeeleAttack >= timeToAttack)
            {
                timerMeeleAttack = 0f;
                meeleAttacking = false;
                meeleAttackArea.SetActive(false);
            }
        }
    }

    private void MeeleAttack(Vector3 vectorAttack)
    {
        meeleAttackArea.transform.position = transform.position + vectorAttack;
        meeleAttackArea.transform.rotation = new Quaternion(0, 0, 0, 0);
        meeleAttackArea.transform.RotateAround(meeleAttackArea.transform.position, new Vector3(0, 0, 1), Mathf.Atan2(vectorAttack.y, vectorAttack.x) / Mathf.PI * 180.0f);
        meeleAttacking = true;
        meeleAttackArea.SetActive(meeleAttacking);
    }

    private void Projectile()
    {
        GameObject projectile = Instantiate(Bullet, transform.position, Quaternion.identity);
    }
}
