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
    public GameObject ProjectileIcon;
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
    public float radius = 1.0f;
    public float stompingDamageForce = 15.0f;
    public GameObject StompAnimation;
    public GameObject StompIcon;

    private bool ableToMissile = false;
    private float timerMeeleAttack = 0f;
    private float timerMissileAttack = 0f;
    private float timerProjectile = 0f;
    private float originalGravity;
    private Rigidbody2D rb;

    private Subscription<PauseEvent> pauseEventSubscription;
    private Subscription<ResumeEvent> resumeEventSubscription;

    // Start is called before the first frame update
    void Start()
    {
        meeleAttackArea = transform.GetChild(0).gameObject;
        pauseEventSubscription = EventBus.Subscribe<PauseEvent>(_OnPause);
        resumeEventSubscription = EventBus.Subscribe<ResumeEvent>(_OnResume);
        rb = GetComponent<Rigidbody2D>();
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
            TrailRenderer tr = GetComponent<TrailRenderer>();
            tr.emitting = false;
            rb.gravityScale = originalGravity;
            isStomping = false;
            GetComponent<Player>().IsInvincible = false;
            canStomp = true;
            GetComponent<PlayerMovement>().enabled = true;
            StompDamage(new Vector2(transform.position.x, transform.position.y - transform.lossyScale.y / 2));
            StompAnimation.SetActive(true);
            StartCoroutine(SetAnimation());
        } else if (isStomping)
        {
            rb.velocity = new Vector2(0, -1) * stompingPower;
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
                MissileAttackIcon.transform.Find("cooldown").gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && ableToMissile)
        {
            Shoot(vectorAttack);
            timerMissileAttack = 0;
            ableToMissile = false;
            GameObject coolDown = MissileAttackIcon.transform.Find("cooldown").gameObject;
            coolDown.SetActive(true);
            coolDown.GetComponent<Animator>().speed = 4.0f / ShootCd;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !meeleAttacking)
        {
            MeeleAttack(vectorAttack);
        }
        if (Input.GetButtonDown("Stomp") && canStomp && StompEnabled && !GetComponent<PlayerMovement>().GetIsGrounded())
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
        TrailRenderer tr = GetComponent<TrailRenderer>();
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, -1) * stompingPower;
        tr.emitting = true;
        GetComponent<Player>().IsInvincible = true;
        GetComponent<PlayerMovement>().enabled = false;
    }

    private void StompDamage(Vector2 pos)
    {
        GameObject enemyManager = GameObject.Find("Enemies");

        foreach (Transform child in enemyManager.transform)
        {
            if (Vector2.Distance(pos, child.position) < radius)
            {
                Vector2 dir = (new Vector2(child.position.x, child.position.y) - pos).normalized;
                child.gameObject.GetComponent<EnemyMovement>().Stun(0.1f);
                Rigidbody2D childRb = child.gameObject.GetComponent<Rigidbody2D>();
                if (childRb != null)
                    childRb.AddForce(dir * stompingDamageForce, ForceMode2D.Impulse);
                HealthSystemForDummies health = child.gameObject.GetComponent<HealthSystemForDummies>();
                int damage = GetComponent<Player>().attack;
                health.AddToCurrentHealth(-damage);
                if (health.CurrentHealth <= 0)
                {
                    child.GetComponent<Enemy>().Dead();
                }
            }
        }
    }

    private IEnumerator SetAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        StompAnimation.SetActive(false);
    }
}
