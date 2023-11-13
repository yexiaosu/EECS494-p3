using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Enemy : MonoBehaviour
{
    public GameObject[] collectiblePrefabs;
    public GameObject MeeleHitEffect;
    public GameObject ProjectileHitEffect;
    public GameObject MissleHitEffect;

    private bool isPlayingMeeleHitEffect = false;
    private bool isPlayingProjectileHitEffect = false;
    private bool isPlayingMissleHitEffect = false;
    private float timeToAttack = .25f;
    private float timerMeeleAttack = 0;
    private float timerProjectileAttack = 0;
    private float timerMissleAttack = 0;
    private GameObject MeeleHitEffectObject;
    private GameObject ProjectileHitEffectObject;
    private GameObject MissleHitEffectObject;

    void Start()
    {
        MeeleHitEffectObject = Instantiate(MeeleHitEffect, transform.position + new Vector3(0.4f, 0.3f, 0), Quaternion.identity);
        MeeleHitEffectObject.transform.parent = transform;
        MeeleHitEffectObject.SetActive(false);
        ProjectileHitEffectObject = Instantiate(ProjectileHitEffect, transform.position, Quaternion.identity);
        ProjectileHitEffectObject.transform.parent = transform;
        ProjectileHitEffectObject.SetActive(false);
        MissleHitEffectObject = Instantiate(MissleHitEffect, transform.position, Quaternion.identity);
        MissleHitEffectObject.transform.parent = transform;
        MissleHitEffectObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayingMeeleHitEffect)
        {
            timerMeeleAttack += Time.deltaTime;
            if (timerMeeleAttack >= timeToAttack)
            {
                timerMeeleAttack = 0f;
                isPlayingMeeleHitEffect = false;
                MeeleHitEffectObject.SetActive(false);
            }
        }
        if (isPlayingProjectileHitEffect)
        {
            timerProjectileAttack += Time.deltaTime;
            if (timerProjectileAttack >= timeToAttack)
            {
                timerProjectileAttack = 0f;
                isPlayingProjectileHitEffect = false;
                ProjectileHitEffectObject.SetActive(false);
            }
        }
        if (isPlayingMissleHitEffect)
        {
            timerMissleAttack += Time.deltaTime;
            if (timerMissleAttack >= timeToAttack)
            {
                timerMissleAttack = 0f;
                isPlayingMissleHitEffect = false;
                MissleHitEffectObject.SetActive(false);
            }
        }
    }

    public void GetMeeleHit()
    {
        MeeleHitEffectObject.SetActive(true);
        isPlayingMeeleHitEffect = true;
    }

    public void GetProjectileHit(Vector3 attackPos)
    {
        ProjectileHitEffectObject.SetActive(true);
        ProjectileHitEffectObject.transform.position = attackPos;
        isPlayingProjectileHitEffect = true;
    }

    public void GetMissleHit(Vector3 vectorAttack, Vector3 attackPos)
    {
        MissleHitEffectObject.SetActive(true);
        MissleHitEffectObject.transform.position = attackPos;
        MissleHitEffectObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        MissleHitEffectObject.transform.RotateAround(MissleHitEffectObject.transform.position, new Vector3(0, 0, 1), Mathf.Atan2(-vectorAttack.y, -vectorAttack.x) / Mathf.PI * 180.0f);
        isPlayingMissleHitEffect = true;
    }

    public void Dead()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        if (collectiblePrefabs.Length > 0)
        {
            int roll = Random.Range(0, 100);
            if (roll < 60)
            {
                SpawnCollectible(collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)], gameObject.transform.position);
            }
        }
        Destroy(gameObject, 0.25f);
    }

    private void SpawnCollectible(GameObject collectible, Vector3 pos)
    {
        Instantiate(collectible, pos, Quaternion.identity);
    }
}
