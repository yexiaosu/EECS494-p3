using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrackingBullet : MonoBehaviour
{
    public float speed = 3.0f;
    public float damageFactor = 0.4f;

    private GameObject minDisEnemy;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float minDistance = 100.0f;
        GameObject enemyManager = GameObject.Find("Enemies");
        foreach (Transform child in enemyManager.transform)
        {
            if (Vector2.Distance(transform.position, child.position) < minDistance)
            {
                minDistance = Vector2.Distance(transform.position, child.position);
                minDisEnemy = child.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (minDisEnemy == null || Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > 20.0f)
        {
            Destroy(gameObject);
        }
        else
        {
            rb.velocity = speed * (minDisEnemy.transform.position - transform.position).normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
        GameObject sender = this.gameObject;

        if (gameObject.CompareTag("Enemy"))
        {
            int amount = Mathf.FloorToInt(GameObject.Find("Player").GetComponent<Player>().attack * damageFactor);
            gameObject.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(-amount);
            collision.GetComponent<Enemy>().GetProjectileHit(sender.transform.position);
            gameObject.GetComponent<KnockBack>().PlayFeedback(sender);
            if (gameObject.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
                gameObject.GetComponent<Enemy>().Dead();
            Destroy(sender);
        }
    }
}
