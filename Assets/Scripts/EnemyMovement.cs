using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    public bool isStunned;
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
}
