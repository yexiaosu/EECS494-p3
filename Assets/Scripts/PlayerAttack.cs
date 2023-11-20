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
    public GameObject MissileAttackIcon;
    // meele attack
    private GameObject meeleAttackArea = default;
    private bool meeleAttacking = false;
    private float meeleAttackingTime = .3f;
    // stomp
    public bool StompEnabled = false;
    private bool canStomp = true;
    private bool isStomping = false;
    private float stompingPower = 25.0f;

    private bool ableToMissile = false;
    private float timerMeeleAttack = 0f;
    private float timerMissileAttack = 0f;
    private float timerProjectile = 0f;
    private float originalGravity;

    private Subscription<PauseEvent> pauseEventSubscription;
    private Subscription<ResumeEvent> resumeEventSubscription;

    // Start is called before the first frame update
    void Start()
    {
        meeleAttackArea = transform.GetChild(0).gameObject;
        pauseEventSubscription = EventBus.Subscribe<PauseEvent>(_OnPause);
        resumeEventSubscription = EventBus.Subscribe<ResumeEvent>(_OnResume);
        MissileAttackIcon.SetActive(false);
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

        if (isStomping && GetComponent<PlayerMovement>().GetIsGrounded())
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            TrailRenderer tr = GetComponent<TrailRenderer>();
            tr.emitting = false;
            rb.gravityScale = originalGravity;
            isStomping = false;
            GetComponent<Player>().IsInvincible = false;
            canStomp = true;
            GetComponent<PlayerMovement>().enabled = true;
        }

        if (ProjectileEnabled)
        {
            timerProjectile += Time.deltaTime;
            if (timerProjectile > ProjectileCd)
            {
                Projectile();
                timerProjectile = 0;
            }
        }
        if (MissileAttackEnabled)
        {
            timerMissileAttack += Time.deltaTime;
            if (timerMissileAttack > ShootCd)
            {
                ableToMissile = true;
                MissileAttackIcon.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && ableToMissile)
        {
            Shoot(vectorAttack);
            timerMissileAttack = 0;
            ableToMissile = false;
            GameObject coolDown = MissileAttackIcon.transform.GetChild(1).gameObject;
            coolDown.SetActive(true);
            coolDown.GetComponent<Animator>().speed = 4.0f / ShootCd;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !meeleAttacking)
        {
            MeeleAttack(vectorAttack);
        }
        if (Input.GetKey(KeyCode.S) && canStomp && StompEnabled)
        {
            Stomp();
        }
        if (meeleAttacking)
        {
            timerMeeleAttack += Time.deltaTime;
            if (timerMeeleAttack >= meeleAttackingTime)
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
        GameObject bullet = Instantiate(ProjectileBullet, transform.position, Quaternion.identity);
        bullet.GetComponent<TrackingBullet>().damageFactor = ProjectileDamageFactor;
        bullet.GetComponent<TrackingBullet>().speed = ProjectileSpeed;
    }

    private void Shoot(Vector3 vectorAttack)
    {
        GameObject bullet = Instantiate(MissileBullet, transform.position + vectorAttack, Quaternion.identity);
        bullet.transform.RotateAround(bullet.transform.position, new Vector3(0, 0, 1), Mathf.Atan2(vectorAttack.y, vectorAttack.x) / Mathf.PI * 180.0f);
        bullet.GetComponent<ShootingBullet>().dir = vectorAttack;
        bullet.GetComponent<ShootingBullet>().damageFactor = MissileAttackDamageFactor;
        bullet.GetComponent<ShootingBullet>().speed = MissileAttackSpeed;
    }

    private void _OnPause(PauseEvent e)
    {
        this.enabled = false;
    }

    private void _OnResume(ResumeEvent e)
    {
        this.enabled = true;
    }

    private void Stomp()
    {
        canStomp = false;
        isStomping = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        TrailRenderer tr = GetComponent<TrailRenderer>();
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, -1) * stompingPower;
        tr.emitting = true;
        GetComponent<Player>().IsInvincible = true;
        GetComponent<PlayerMovement>().enabled = false;
    }
}
