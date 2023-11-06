using UnityEngine;

public class ItemShopDoor : MonoBehaviour
{
    public GameObject enterPrompt; // Assign a UI element that prompts the user to press 'E'.
    private bool isPlayerInTrigger = false;

    private void Start()
    {
        // Ensure the prompt is not visible at the start.
        if (enterPrompt != null)
            enterPrompt.SetActive(false);
    }

    private void Update()
    {
        // Check if the player is in the trigger and has pressed the 'E' key
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            TeleportPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has the PlayerMovement script attached
        if (other.GetComponent<PlayerMovement>() != null)
        {
            isPlayerInTrigger = true;
            if (enterPrompt != null)
                enterPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the colliding object has the PlayerMovement script attached
        if (other.GetComponent<PlayerMovement>() != null)
        {
            isPlayerInTrigger = false;
            if (enterPrompt != null)
                enterPrompt.SetActive(false);
        }
    }

    private void TeleportPlayer()
    {
        Vector3 hardCodedLocation = new Vector3(45.8f, 7.88f, 0f); 
        Camera mainCamera = Camera.main;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 playerPosition =player.transform.position;
        Vector3 cameraPositionBeforeEntering = mainCamera.transform.position;

        player.transform.position = hardCodedLocation;



        EventBus.Publish(new EnterItemShopEvent(playerPosition, cameraPositionBeforeEntering));


        if (enterPrompt != null)
            enterPrompt.SetActive(false);

    }
}
