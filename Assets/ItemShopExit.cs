using UnityEngine;

public class ItemShopExit : MonoBehaviour
{
    public GameObject exitPrompt; // UI prompt for the player to press a key to exit
    private bool isPlayerInTrigger = false;
    private Vector3 returnPosition; // This will store the player's position before entering the shop
    private Vector3 returnCameraPosition; // To store the camera's position before entering the shop


    private Subscription<EnterItemShopEvent> enterShopEventSubscription;

    private void Awake()
    {
        // Subscribe to EnterItemShopEvent to get the player's position before they entered the shop
        enterShopEventSubscription = EventBus.Subscribe<EnterItemShopEvent>(OnEnterItemShop);
    }

    private void Start()
    {
        // Ensure the prompt is not visible at the start
        if (exitPrompt != null)
            exitPrompt.SetActive(false);
    }

    private void OnEnterItemShop(EnterItemShopEvent evt)
    {
        // Store the player's position before they entered the shop
        returnPosition = evt.playerPositionBeforeEntering;
        returnCameraPosition = evt.cameraPositionBeforeEntering;

    }

    private void Update()
    {
        // Check if the player is in the trigger and has pressed the 'E' key
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            ExitItemShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has the Player tag
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            // Show the exit prompt
            if (exitPrompt != null)
                exitPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the colliding object has the Player tag
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            // Hide the exit prompt
            if (exitPrompt != null)
                exitPrompt.SetActive(false);
        }
    }

    private void ExitItemShop()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Camera mainCamera = Camera.main;

        if (player != null)
        {
            // Teleport the player back to the original position
            player.transform.position = returnPosition;
            mainCamera.transform.position = returnCameraPosition;


            // Hide the exit prompt
            if (exitPrompt != null)
                exitPrompt.SetActive(false);

            // Publish the ExitItemShopEvent indicating that the player has left the shop
            EventBus.Publish(new ExitItemShopEvent());
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (enterShopEventSubscription != null)
        {
            EventBus.Unsubscribe(enterShopEventSubscription);
        }
    }
}

