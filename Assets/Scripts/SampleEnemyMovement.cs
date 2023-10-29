using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEnemyMovement : WalkMovement
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        time = 0;
        changeDirectionTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
