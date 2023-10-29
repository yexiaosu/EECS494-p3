using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate()
    {
        if (player)
        {
            transform.position = new Vector3(transform.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
    }
}
