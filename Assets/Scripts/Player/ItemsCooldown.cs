using System.Collections;
using UnityEngine;

public class ItemsCooldown : MonoBehaviour
{
    [SerializeField] private CooldownUI cooldownUI;
    public static ItemsCooldown Instance;
    private Coroutine cooldownCoroutine;
    private bool onCooldown = false;

    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void SetOnCooldown(float time = 0)
    {
        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);
        cooldownCoroutine = StartCoroutine(Cooldown(time));
    }

    public bool IsOnCooldown()
    {
        return onCooldown;
    }

    private IEnumerator Cooldown(float time)
    {
        onCooldown = true;
        float currentTime = time;
        while (currentTime > 0)
        {
            cooldownUI.SetCooldownFill(currentTime / time);
            currentTime -= Time.deltaTime;  
            yield return new WaitForFixedUpdate();
        }
        onCooldown = false;
    }
}
