using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeat : MonoBehaviour
{
    public float offset;
    private GameObject player;

    // Update is called once per frame

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (Mathf.Abs(player.transform.position.y - transform.position.y) > offset)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        float dir = (player.transform.position.y - transform.position.y) > 0 ? 1 : -1;
        Vector3 initPos = transform.position;
        transform.position = transform.position + new Vector3(0, offset * 2f, 0) * dir;
        Vector3 currPos = transform.position;
        EventBus.Publish<RepeatEvent>(new RepeatEvent(currPos, initPos));
    }
}

public class RepeatEvent
{
    public Vector3 currPos;
    public Vector3 initPos;
    public RepeatEvent(Vector3 _currPos, Vector3 _initPos) { currPos = _currPos; initPos = _initPos; }
}
