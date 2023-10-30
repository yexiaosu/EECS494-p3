using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject[] collectiblePrefabs;

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
        Destroy(gameObject);
    }

    private void SpawnCollectible(GameObject collectible, Vector3 pos)
    {
        Instantiate(collectible, pos, Quaternion.identity);
    }
}
