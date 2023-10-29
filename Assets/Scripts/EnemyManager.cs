using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float offset;
    public float spawnCd = 1.0f;
    [SerializeField] public List<GOArray> enemyPrefabs = new List<GOArray>(); // two lists, the first one contains walk enemies, the second one contains fly enemies

    private Subscription<RepeatEvent> repeatEventSubscription;
    private Camera cam;
    private float camHalfWidth;
    private float camHalfHeight;

    void Start()
    {
        repeatEventSubscription = EventBus.Subscribe<RepeatEvent>(_OnRepeat);
        cam = Camera.main;
        StartCoroutine(SpawnEnemy());
        camHalfWidth = cam.aspect * cam.orthographicSize;
        camHalfHeight = cam.orthographicSize;
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
        InstantiateWalkEnemy(1);
        InstantiateFlyEnemy(2);
    }

    private void InstantiateWalkEnemy(int num = 1)
    {
        List<GameObject> platformsInView = getPlatformsInView();
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

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(spawnCd);
        if (UnityEngine.Random.Range(0.0f, 100.0f) < 40.0f)
            // spawn walk enemy
            InstantiateWalkEnemy();
        else
            // spawn fly enemy
            InstantiateFlyEnemy();
        StartCoroutine(SpawnEnemy());
    }

    private List<GameObject> getPlatformsInView()
    {
        GameObject platforms = GameObject.Find("Platforms");
        List<GameObject> platformsInView = new List<GameObject>();
        foreach (Transform child in platforms.transform)
        {
            if (child.position.y > cam.transform.position.y - camHalfHeight && child.position.y < cam.transform.position.y + camHalfHeight)
            {
                // the platform is in view
                platformsInView.Add(child.gameObject);
            }
        }
        return platformsInView;
    }
}

[System.Serializable]
public class GOArray
{
    [SerializeField]
    public List<GameObject> _gameObjects;

    // optionally some other fields
}
