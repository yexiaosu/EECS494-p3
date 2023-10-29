using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : EnemyMovement
{
    [SerializeField] protected float speed = 2;
    [SerializeField] protected float maxDirectionChangeTime;
    [SerializeField] protected float minDirectionChangeTime;
    [SerializeField] protected float flyingTime = 23.0f;
    [SerializeField] protected float stopTime = 2.0f;

    protected Rigidbody2D rb;
    protected float changeDirectionTime;
    protected float time;
    protected Vector2 input;
    protected bool isStop = false;
    private float leftEdge = -10.0f;
    private float rightEdge = 10.0f;
    protected float bottromEdge = 0.0f;
    protected float topEdge = 8.0f;

    private bool canChangeDir = true;

    protected override Vector2 GetInput(Vector2 rawInput)
    {
        float horizonInput = rawInput.x;
        float verticalInput = rawInput.y;
        if (canChangeDir)
        {
            bool flag = false;
            if ((transform.position.x - leftEdge < 0.001 && horizonInput < 0) || (rightEdge - transform.position.x < 0.001 && horizonInput > 0))
            {
                horizonInput = -1 * horizonInput;
                flag = true;
            }
            if ((transform.position.y - bottromEdge < 0.001 && verticalInput < 0) || (topEdge - transform.position.y < 0.001 && verticalInput < 0))
            {
                verticalInput = -1 * verticalInput;
                flag = true;
            }
            if (flag)
            {
                canChangeDir = false;
                StartCoroutine(StopChangeDir());
            }
        }
        return new Vector2(horizonInput, verticalInput);
    }

    protected virtual void Move()
    {
        if (isStop || isStunned)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        time += Time.fixedDeltaTime;
        if (time >= changeDirectionTime || isOutOfRange())
        {
            time = 0;
            changeDirectionTime = Random.Range(minDirectionChangeTime, maxDirectionChangeTime);
            input = GenerateInput();
        }
        Vector2 velocity = GetInput(input).normalized;
        rb.velocity = velocity * speed;
    }

    private IEnumerator Stop()
    {
        yield return new WaitForSeconds(stopTime);
        isStop = false;
        StartCoroutine(Fly());
    }

    protected IEnumerator Fly()
    {
        yield return new WaitForSeconds(flyingTime);
        isStop = true;
        StartCoroutine(Stop());
    }

    protected bool isOutOfRange()
    {
        if (transform.position.x - leftEdge < 0.001 || rightEdge - transform.position.x < 0.001 || transform.position.y - bottromEdge < 0.001 || topEdge - transform.position.y < 0.001)
            return true;
        else
            return false;
    }

    private IEnumerator StopChangeDir()
    {
        yield return new WaitForSeconds(1);
        canChangeDir = true;
    }
}
