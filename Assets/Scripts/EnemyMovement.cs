using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    public bool isStunned;
    protected Rigidbody2D rb;
    protected float changeDirectionTime;
    protected float time;
    protected Vector2 input;
    protected Subscription<PauseEvent> pauseEventSubscription;
    protected Subscription<ResumeEvent> resumeEventSubscription;

    protected virtual Vector2 GenerateInput()
    {
        float horizonInput = Random.Range(-1f, 1f);
        float verticalInput = Random.Range(-1f, 1f);

        return new Vector2(horizonInput, verticalInput);
    }

    public void Stun(float waitTime)
    {
        Debug.Log("Applying stun to " + gameObject.name);
        StartCoroutine(ApplyStun(waitTime));
    }


    protected IEnumerator ApplyStun(float waitTime)
    {
        if (isStunned)
            yield break;
        isStunned = true;
        yield return new WaitForSeconds(waitTime);
        isStunned = false;
    }

    protected abstract Vector2 GetInput(Vector2 rawInput);

    protected void _OnPause(PauseEvent e)
    {
        this.enabled = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
    }

    protected void _OnResume(ResumeEvent e)
    {
        this.enabled = true;
        rb.gravityScale = 1;
    }
}
