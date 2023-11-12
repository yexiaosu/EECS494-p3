using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielder : MonoBehaviour
{
    public int MaxShieldTimes = 1;

    private Subscription<ShieldRecoverEvent> shieldRecoverEventSubscription;
    private int currShieldTimes = 1;

    // Start is called before the first frame update
    void Start()
    {
        currShieldTimes = MaxShieldTimes;
        shieldRecoverEventSubscription = EventBus.Subscribe<ShieldRecoverEvent>(_OnShieldRecover);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
        GameObject sender = this.gameObject;

        if (gameObject.CompareTag("Enemy"))
        {
            if (gameObject.transform.position.y < sender.transform.position.y - GameObject.Find("Player").transform.lossyScale.y)
                // player jumps to the enemy's head, shield will not take effect because this is a way to attack
                return;
            currShieldTimes--;
            gameObject.GetComponent<KnockBack>().PlayFeedback(sender);
            if (currShieldTimes <= 0)
            {
                EventBus.Publish<ShieldBrokenEvent>(new ShieldBrokenEvent());
            }
        }
    }

    private void _OnShieldRecover(ShieldRecoverEvent e)
    {
        currShieldTimes = MaxShieldTimes;
    }
}

public class ShieldBrokenEvent
{
    public ShieldBrokenEvent() {  }
}

