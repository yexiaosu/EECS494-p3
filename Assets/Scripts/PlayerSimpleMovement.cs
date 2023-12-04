using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimpleMovement : MonoBehaviour
{
    public bool AbleToTriggerDialog = false;
    public GameObject Dialog;

    private float moveSpeed = 4.0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing");
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        // Horizontal Movement
        float moveX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        if (AbleToTriggerDialog && Input.GetKeyDown(KeyCode.F))
        {
            Dialog.SetActive(true);
            Dialog.transform.Find("Dialog").Find("1").gameObject.SetActive(true);
        }
    }
}
