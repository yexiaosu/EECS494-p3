using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : EnemyMovement
{
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected float speed = 2;
    [SerializeField] protected float maxDirectionChangeTime;
    [SerializeField] protected float minDirectionChangeTime;

    protected Rigidbody2D rb;
    protected Vector2 input;
    protected float changeDirectionTime;
    protected float time;
    protected float movementPrecision = 0.05f;

    protected override Vector2 GenerateInput()
    {
        float horizonInput = UnityEngine.Random.Range(-1f, 1f);

        return new Vector2(horizonInput, 0);
    }

    protected override Vector2 GetInput(Vector2 rawInput)
    {
        float horizonInput = rawInput.x;

        return new Vector2(horizonInput, 0);
    }

    protected virtual void Move()
    {
        if (isStunned)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        time += Time.deltaTime;
        if (time >= changeDirectionTime)
        {
            time = 0;
            changeDirectionTime = UnityEngine.Random.Range(minDirectionChangeTime, maxDirectionChangeTime);
            input = GenerateInput();
        }
        if (hasWallinFront(transform.position, new Vector3(input.x, input.y, 0)))
        {
            time = 0;
            changeDirectionTime = UnityEngine.Random.Range(minDirectionChangeTime, maxDirectionChangeTime);
            input = -1 * input;
        }
        Vector2 velocity = GetInput(input).normalized;
        rb.velocity = velocity * speed;
    }

    protected bool hasWallinFront(Vector3 origin, Vector3 direction)
    {
        if (Physics2D.Raycast(origin, direction.normalized, 0.3f, layerMask).collider != null)
            return true;
        else
            return false;
    }
}
