using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject ProjectileBullet;
    public GameObject MissileBullet;
    public GameObject DirectionSprite;
    // tracking projectile
    public bool ProjectileEnabled = false;
    public float ProjectileCd = 2.0f;
    public float ProjectileDamageFactor = 0.4f;
    public float ProjectileSpeed = 3.0f;
    // missile attack
    public bool MissileAttackEnabled = false;
    public float ShootCd = 3.0f;
    public float MissileAttackSpeed = 5.0f;
    public float MissileAttackDamageFactor = 1.0f;

    private GameObject meeleAttackArea = default;
    private bool meeleAttacking = false;
    private bool ableToMissile = false;
    private float timeToAttack = .3f;
    private float timerMeeleAttack = 0f;
    private float timerMissileAttack = 0f;
    private float timerProjectile = 0f;


    public float MeeleAttackCooldown = 1.0f;
    public float MissileAttackCooldown = 1.0f;

    private float meeleAttackTimer = 0f;
    private float missileAttackTimer = 0f;
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
        DirectionSprite.transform.rotation = new Quaternion(0, 0, 0, 0);
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

        // Missile attack cooldown check
        if (MissileAttackEnabled && !ableToMissile)
        {
            missileAttackTimer += Time.deltaTime;
            if (missileAttackTimer > MissileAttackCooldown)
            {
                ableToMissile = true;
                missileAttackTimer = 0f; // Reset the missile attack timer
            }
        }

        // Check for missile attack input
        if (Input.GetKeyDown(KeyCode.Mouse1) && ableToMissile)
        {
            Shoot(vectorAttack);
            ableToMissile = false;
        }

        // Melee attack cooldown check
        if (meeleAttacking)
        {
            meeleAttackTimer += Time.deltaTime;
            if (meeleAttackTimer >= MeeleAttackCooldown)
            {
                meeleAttacking = false;
                meeleAttackArea.SetActive(false);
                meeleAttackTimer = 0f; // Reset the melee attack timer
            }
        }

        // Check for melee attack input
        if (Input.GetKeyDown(KeyCode.Mouse0) && !meeleAttacking)
        {
            MeeleAttack(vectorAttack);
            meeleAttackTimer = 0f; // Start the melee attack timer
        }
    }


    private void MeeleAttack(Vector3 vectorAttack)
    {
        if (!meeleAttacking) // Check if melee attack is available
        {
            meeleAttackArea.transform.position = transform.position + vectorAttack;
            meeleAttackArea.transform.rotation = new Quaternion(0, 0, 0, 0);
            meeleAttackArea.transform.RotateAround(meeleAttackArea.transform.position, new Vector3(0, 0, 1), Mathf.Atan2(vectorAttack.y, vectorAttack.x) / Mathf.PI * 180.0f);
            meeleAttacking = true;
            meeleAttackArea.SetActive(meeleAttacking);
            meeleAttackTimer = 0f; // Start the melee attack timer
        }
    }

    private void Projectile()
    {
        GameObject bullet = Instantiate(ProjectileBullet, transform.position, Quaternion.identity);
        bullet.GetComponent<TrackingBullet>().damageFactor = ProjectileDamageFactor;
        bullet.GetComponent<TrackingBullet>().speed = ProjectileSpeed;
    }

    private void Shoot(Vector3 vectorAttack)
    {
        if (ableToMissile) // Check if missile attack is available
        {
            GameObject bullet = Instantiate(MissileBullet, transform.position + vectorAttack, Quaternion.identity);
            bullet.transform.RotateAround(bullet.transform.position, new Vector3(0, 0, 1), Mathf.Atan2(vectorAttack.y, vectorAttack.x) / Mathf.PI * 180.0f);
            bullet.GetComponent<ShootingBullet>().dir = vectorAttack;
            bullet.GetComponent<ShootingBullet>().damageFactor = MissileAttackDamageFactor;
            bullet.GetComponent<ShootingBullet>().speed = MissileAttackSpeed;

            ableToMissile = false;
            missileAttackTimer = 0f; // Start the missile attack timer
        }
    }
}
