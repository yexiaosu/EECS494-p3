using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBullet : MonoBehaviour
{
    public float speed = 5.0f;
    public Vector2 dir = new Vector2(0, 0);

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed * dir.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > 20.0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
        GameObject sender = this.gameObject;

        if (gameObject.CompareTag("Enemy"))
        {
            int amount = GameObject.Find("Player").GetComponent<Player>().attack;
            gameObject.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(-amount);
            gameObject.GetComponent<KnockBack>().PlayFeedback(sender);
            if (gameObject.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
                gameObject.GetComponent<Enemy>().Dead();
            Destroy(sender);
        }
    }
}
