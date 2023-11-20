using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterBossDoor : MonoBehaviour
{
    public GameObject afterBossPlatformPrefab; // Assign in Inspector
    private RoomManager roomManager;

    private void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            SceneManager.LoadScene("Level_2");

        }
    }

    
}
