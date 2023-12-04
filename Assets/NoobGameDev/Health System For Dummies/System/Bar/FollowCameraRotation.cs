using UnityEngine;

public class FollowCameraRotation : MonoBehaviour
{
    [SerializeField] Transform target;

    void Start()
    {
        if (target == null)
        {
            target = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            target = Camera.main.transform;
        }
        transform.LookAt(transform.position + target.forward);
    }
}
