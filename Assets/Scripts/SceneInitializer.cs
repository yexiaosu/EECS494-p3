using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    void Start()
    {
        MovePlayerToSpawnPoint();
        UIManager.Instance.ChangeOffset(400);
    }

    private void MovePlayerToSpawnPoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Set player position to (0, 1, 0)
            player.transform.position = new Vector3(0, 9, 0);
        }
        else
        {
            Debug.LogError("Player not found in the scene");
        }
    }
}
