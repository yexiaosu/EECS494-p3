using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrackingBullet : MonoBehaviour
{
    public float speed = 3.0f;
    public float damageFactor = 0.2f;

    private Camera mainCamera;
    private GameObject minDisEnemy;
    private Rigidbody2D rb;
    private Subscription<PauseEvent> pauseEventSubscription;
    private Subscription<ResumeEvent> resumeEventSubscription;
    private bool isPause = false;
    private Vector2 recordSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FindCamera();

        FindClosestVisibleEnemy();

        pauseEventSubscription = EventBus.Subscribe<PauseEvent>(_OnPause);
        resumeEventSubscription = EventBus.Subscribe<ResumeEvent>(_OnResume);
    }


    void FindCamera()
    {
        GameObject cameraByName = GameObject.Find("Main Camera");
        if (cameraByName != null)
            mainCamera = cameraByName.GetComponent<Camera>();

        if (mainCamera == null)
        {
            GameObject cameraByTag = GameObject.FindGameObjectWithTag("Main Camera");
            if (cameraByTag != null)
                mainCamera = cameraByTag.GetComponent<Camera>();
        }

        if (mainCamera == null)
        {
            CameraTracking cameraTrackingScript = FindObjectOfType<CameraTracking>();
            if (cameraTrackingScript != null)
                mainCamera = cameraTrackingScript.GetComponent<Camera>();
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Make sure it's tagged correctly or has the right script.");
        }
    }

    void FindClosestVisibleEnemy()
    {
        GameObject enemyManager = GameObject.Find("Enemies");
        float minDistance = Mathf.Infinity;
        minDisEnemy = null;

        foreach (Transform child in enemyManager.transform)
        {
            if (IsEnemyVisible(child.gameObject))
            {
                float distance = Vector2.Distance(transform.position, child.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minDisEnemy = child.gameObject;
                }
            }
        }
    }

    bool IsEnemyVisible(GameObject enemy)
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(enemy.transform.position);
        return viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && viewPos.z > 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPause)
            return;

        if (minDisEnemy == null)
        {
            FindClosestVisibleEnemy();
        }

        if (minDisEnemy != null)
        {
            rb.velocity = speed * (minDisEnemy.transform.position - transform.position).normalized;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        GameObject sender = this.gameObject;

        if (target.CompareTag("Enemy") && target.GetComponent<Enemy>() != null)
        {
            int amount = Mathf.FloorToInt(GameObject.Find("Player").GetComponent<Player>().attack * damageFactor);
            target.GetComponent<HealthSystemForDummies>().AddToCurrentHealth(-amount);
            if (target.GetComponent<Enemy>() != null)
                target.GetComponent<Enemy>().GetProjectileHit(sender.transform.position);
            if (target.GetComponent<KnockBack>() != null)
                target.GetComponent<KnockBack>().PlayFeedback(sender);
            if (target.GetComponent<HealthSystemForDummies>().CurrentHealth <= 0)
                target.GetComponent<Enemy>().Dead();
            Destroy(sender);
        }
        else if (target.GetComponent<Platform>() != null)
            Destroy(sender);
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
