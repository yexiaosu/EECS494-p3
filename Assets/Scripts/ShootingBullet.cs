using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBullet : MonoBehaviour
{
    public float speed = 5.0f;
    public float damageFactor = 1.0f;
    public Vector2 dir = new Vector2(0, 0);

    private Rigidbody2D rb;
    private Subscription<PauseEvent> pauseEventSubscription;
    private Subscription<ResumeEvent> resumeEventSubscription;
    private bool isPause = false;
    private Vector2 recordSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed * dir.normalized;
        pauseEventSubscription = EventBus.Subscribe<PauseEvent>(_OnPause);
        resumeEventSubscription = EventBus.Subscribe<ResumeEvent>(_OnResume);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause)
            return;

        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > 20.0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        GameObject sender = this.gameObject;

        // Check if the collided object has a Platform script
        if (collidedObject.GetComponent<Platform>() != null)
        {
            Destroy(sender);
            return; // Early exit to prevent further execution
        }

        if (collidedObject.CompareTag("Enemy"))
        {
            int amount = Mathf.FloorToInt(GameObject.Find("Player").GetComponent<Player>().attack * damageFactor);
            collidedObject.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(-amount);
            collision.GetComponent<Enemy>().GetMissleHit(dir, sender.transform.position);
            collidedObject.GetComponent<KnockBack>().PlayFeedback(sender);
            if (collidedObject.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
                collidedObject.GetComponent<Enemy>().Dead();
            Destroy(sender);
        }
    }

    private void _OnPause(PauseEvent e)
    {
        isPause = true;
        recordSpeed = rb.velocity;
    }

    private void _OnResume(ResumeEvent e)
    {
        isPause = false;
        rb.velocity = recordSpeed;
    }
}
