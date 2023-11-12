using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public GameObject Shield;
    public float ReGeneratedCd = 2.0f;
    public bool ShieldEnabled = false;

    private Subscription<ShieldBrokenEvent> shieldBrokenEventSubscription;
    private float timer = 0;
    private bool isTimerStart = false;

    // Start is called before the first frame update
    void Start()
    {
        shieldBrokenEventSubscription = EventBus.Subscribe<ShieldBrokenEvent>(_OnShieldBroken);
        Shield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerStart)
        {
            timer += Time.deltaTime;
            if (timer > ReGeneratedCd)
            {
                Shield.SetActive(true);
                EventBus.Publish<ShieldRecoverEvent>(new ShieldRecoverEvent());
                timer = 0;
                isTimerStart = false;
            }
        }
    }

    private void _OnShieldBroken(ShieldBrokenEvent e)
    {
        isTimerStart = true;
        Shield.SetActive(false);
    }
}

public class ShieldRecoverEvent
{
    public ShieldRecoverEvent() { }
}
