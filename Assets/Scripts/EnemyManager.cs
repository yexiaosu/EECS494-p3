using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float offset;
    public List<List<GameObject>> enemyPrefabs; // two lists, the first one contains walk enemies, the second one contains fly enemies

    private Subscription<RepeatEvent> repeatEventSubscription;

    void Start()
    {
        repeatEventSubscription = EventBus.Subscribe<RepeatEvent>(_OnRepeat);
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
    }

    private void InstantiateWalkEnemy(Vector3 pos)
    {
        GameObject enemy = Instantiate(enemyPrefabs[0][Random.Range(0, enemyPrefabs[0].Count)], pos, Quaternion.identity);
        enemy.transform.parent = transform;
    }

    private void InstantiateFlyEnemy(Vector3 pos)
    {
        GameObject enemy = Instantiate(enemyPrefabs[1][Random.Range(0, enemyPrefabs[1].Count)], pos, Quaternion.identity);
        enemy.transform.parent = transform;
    }
}
