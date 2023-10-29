using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class SampleFlyEnemyMovement : FlyMovement
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        time = 0;
        changeDirectionTime = 0;
        StartCoroutine(Fly());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
}
