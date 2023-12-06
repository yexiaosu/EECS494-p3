using UnityEngine;
using System.Collections;

public class IceBossLaser : MonoBehaviour
{
    private Animator animator;
    public float laserActiveDuration = 1.5f; // Duration for which the laser stays active
    public Collider2D[] stageColliders;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateLaser()
    {
        gameObject.SetActive(true); // Activate the laser
        Debug.Log("Animation played");

        animator.SetTrigger("Laser"); // Start the laser animation
        StartCoroutine(DeactivateAfterDuration());
    }

    public void ActivateStageCollider(int index)
    {
        for (int i = 0; i < stageColliders.Length; i++)
        {
            stageColliders[i].enabled = (i == index);
        }
    }

    public void FlipColliders(bool shouldFlip)
    {
        foreach (var collider in stageColliders)
        {
            Vector2 offset = collider.offset;
            offset.x = shouldFlip ? -offset.x : offset.x;
            collider.offset = offset;
        }
    }

    private IEnumerator DeactivateAfterDuration()
    {
        yield return new WaitForSeconds(laserActiveDuration); // Wait for the specified duration
        Debug.Log("DEACTIVATED");
        gameObject.SetActive(false); // Deactivate the laser
    }
}
