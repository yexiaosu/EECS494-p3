using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KnockBack : MonoBehaviour
{
    public int strength = 10;
    public float delay = 0.5f;
    public Rigidbody2D rb;

    public UnityEvent OnBegin, OnDone;

    public void PlayFeedback(GameObject sender)
    {
        StopAllCoroutines();
        OnBegin.Invoke();
        float x = transform.position.x - sender.transform.position.x;
        float y = transform.position.y - sender.transform.position.y;
        if (Math.Abs(x) > Math.Abs(y))
            y = 0;
        else
            x = 0;
        Vector3 direction = new Vector3(x, y, 0).normalized;
        rb.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        OnDone.Invoke();
    }
}
