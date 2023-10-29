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

    private bool canChangeDir = false;
    private Vector2 cameraPos;

    protected override Vector2 GetInput(Vector2 rawInput)
    {
        float horizonInput = rawInput.x;
        float verticalInput = rawInput.y;
        if (canChangeDir)
        {
            bool flag = false;
            if ((transform.position.x < cameraPos.x && horizonInput < 0) || (cameraPos.x < transform.position.x && horizonInput > 0))
            {
                horizonInput = -1 * horizonInput;
                flag = true;
            }
            if ((transform.position.y < cameraPos.y && verticalInput < 0) || (cameraPos.y < transform.position.y && verticalInput > 0))
            {
                verticalInput = -1 * verticalInput;
                flag = true;
            }
            if (flag)
                canChangeDir = false;
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
        if (time >= changeDirectionTime)
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MainCamera")
        {
            canChangeDir = true;
            cameraPos = collision.transform.position;
            time = 0;
            changeDirectionTime = Random.Range(minDirectionChangeTime, maxDirectionChangeTime);
            input = GenerateInput();
            Vector2 velocity = GetInput(input).normalized;
            rb.velocity = velocity * speed;
        }
    }
}
