using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float offset;
    public float spawnCd = 1.0f;
    public int maxEnemyCount = 5;
    [SerializeField] public List<GOArray> enemyPrefabs = new List<GOArray>(); // two lists, the first one contains walk enemies, the second one contains fly enemies

    private Subscription<EnterItemShopEvent> enterShopSubscription;
    private Subscription<ExitItemShopEvent> exitShopSubscription;
    private Subscription<RepeatEvent> repeatEventSubscription;
    private Subscription<PauseEvent> pauseEventSubscription;
    private Subscription<ResumeEvent> resumeEventSubscription;
    private Camera cam;
    private float camHalfWidth;
    private float camHalfHeight;
    private float time;
    private bool shouldSpawn = true;

    void Start()
    {
        enterShopSubscription = EventBus.Subscribe<EnterItemShopEvent>(_OnEnterItemShop);
        exitShopSubscription = EventBus.Subscribe<ExitItemShopEvent>(_OnExitItemShop);
        repeatEventSubscription = EventBus.Subscribe<RepeatEvent>(_OnRepeat);
        pauseEventSubscription = EventBus.Subscribe<PauseEvent>(_OnPause);
        resumeEventSubscription = EventBus.Subscribe<ResumeEvent>(_OnResume);
        cam = Camera.main;
        camHalfWidth = cam.aspect * cam.orthographicSize;
        camHalfHeight = cam.orthographicSize;
        time = 0;
    }

    private void FixedUpdate()
    {
        if (!shouldSpawn)
            return;
        time += Time.fixedDeltaTime;
        if (time > spawnCd)
        {
            int enemyCount = 0;
            foreach (Transform child in transform)
            {
                if (IsInView(child.position))
                {
                    enemyCount++;
                }
            }
            if (enemyCount < maxEnemyCount)
            {
                SpawnEnemy();
            }
            time = 0;
        }
    }

    private void _OnRepeat(RepeatEvent e)
    {
        Vector3 initPos = e.initPos;
        // destroy old enemies
        foreach (Transform child in transform)
        {
            if (child.position.y > initPos.y - offset / 2 && child.position.y < initPos.y + offset / 2)
            {
                Destroy(child.gameObject);
            }
        }
        // spawn some new enemies
        //InstantiateWalkEnemy(1);
        //InstantiateFlyEnemy(2);
    }

    private void _OnPause(PauseEvent e)
    {
        shouldSpawn = false;
    }

    private void _OnResume(ResumeEvent e)
    {
        shouldSpawn = true;
    }

    private void _OnEnterItemShop(EnterItemShopEvent e)
    {
        shouldSpawn = false;
    }

    private void _OnExitItemShop(ExitItemShopEvent e)
    {
        shouldSpawn = true;
    }

    private void InstantiateWalkEnemy(int num = 1)
    {
        List<GameObject> platformsInView = GetPlatformsInView();
        Vector3 platformPos = platformsInView[UnityEngine.Random.Range(0, platformsInView.Count)].transform.position;
        for (int i = 0; i < num; i++)
        {
            Vector3 pos = new Vector3(platformPos.x, platformPos.y + 0.6f, 0);
            GameObject enemy = Instantiate(enemyPrefabs[0]._gameObjects[UnityEngine.Random.Range(0, enemyPrefabs[0]._gameObjects.Count)], pos, Quaternion.identity);
            enemy.transform.parent = transform;
        }
    }

    private void InstantiateFlyEnemy(int num = 1)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(cam.transform.position.x - camHalfWidth, cam.transform.position.x + camHalfWidth), UnityEngine.Random.Range(cam.transform.position.y - camHalfHeight, cam.transform.position.y + camHalfHeight), 0);
            GameObject enemy = Instantiate(enemyPrefabs[1]._gameObjects[UnityEngine.Random.Range(0, enemyPrefabs[1]._gameObjects.Count)], pos, Quaternion.identity);
            enemy.transform.parent = transform;
        }
    }

    private void SpawnEnemy()
    {
        if (UnityEngine.Random.Range(0.0f, 100.0f) < 40.0f)
            // spawn walk enemy
            InstantiateWalkEnemy();
        else
            // spawn fly enemy
            InstantiateFlyEnemy();
    }

    private List<GameObject> GetPlatformsInView()
    {
        List<GameObject> platformsInView = new List<GameObject>();
        foreach (Transform child in GameObject.Find("Game Controller").transform)
        {
            GameObject platforms = child.gameObject;
            foreach (Transform c in platforms.transform)
            {
                if (IsInView(c.position))
                {
                    // the platform is in view
                    platformsInView.Add(c.gameObject);
                }
            }
        }
        return platformsInView;
    }

    private bool IsInView(Vector3 pos)
    {
        return (pos.y > cam.transform.position.y - camHalfHeight && pos.y < cam.transform.position.y + camHalfHeight);
    }
}

[System.Serializable]
public class GOArray
{
    [SerializeField]
    public List<GameObject> _gameObjects;

    // optionally some other fields
}
