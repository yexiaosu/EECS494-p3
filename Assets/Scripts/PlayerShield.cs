using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public GameObject Shield;
    public float ReGeneratedCd = 4.0f;
    public int MaxShieldTimes = 1;
    public bool ShieldEnabled = false;
    public GameObject ShieldIcon;

    private Subscription<ShieldBrokenEvent> shieldBrokenEventSubscription;
    private float timer = 0;
    private bool isTimerStart = false;

    // Start is called before the first frame update
    void Start()
    {
        shieldBrokenEventSubscription = EventBus.Subscribe<ShieldBrokenEvent>(_OnShieldBroken);
        Shield.SetActive(false);
        ShieldIcon.SetActive(false);
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
                ShieldIcon.transform.GetChild(1).gameObject.SetActive(false);
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
        StartCoroutine(SetInvincible());
        GameObject coolDown = ShieldIcon.transform.GetChild(1).gameObject;
        coolDown.SetActive(true);
        coolDown.GetComponent<Animator>().speed = 4.0f / ReGeneratedCd;
    }

    private IEnumerator SetInvincible()
    {
        Player player = GetComponent<Player>();
        player.IsInvincible = true;
        yield return new WaitForSeconds(0.5f);
        player.IsInvincible = false;
    }
}

public class ShieldRecoverEvent
{
    public ShieldRecoverEvent() { }
}
