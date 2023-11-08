using UnityEngine;

public class ItemShopExit : MonoBehaviour
{
    public GameObject exitPrompt; 
    private bool isPlayerInTrigger = false;
    private Vector3 returnPosition; 
    private Vector3 returnCameraPosition;


    private Subscription<EnterItemShopEvent> enterShopEventSubscription;

    private void Awake()
    {
        enterShopEventSubscription = EventBus.Subscribe<EnterItemShopEvent>(OnEnterItemShop);
    }

    private void Start()
    {
        if (exitPrompt != null)
            exitPrompt.SetActive(false);
    }

    private void OnEnterItemShop(EnterItemShopEvent evt)
    {
        returnPosition = evt.playerPositionBeforeEntering;
        returnCameraPosition = evt.cameraPositionBeforeEntering;

    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            ExitItemShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
            player.transform.position = returnPosition;
            mainCamera.transform.position = returnCameraPosition;


            if (exitPrompt != null)
                exitPrompt.SetActive(false);

            EventBus.Publish(new ExitItemShopEvent());
        }
    }

    private void OnDestroy()
    {
        if (enterShopEventSubscription != null)
        {
            EventBus.Unsubscribe(enterShopEventSubscription);
        }
    }
}

