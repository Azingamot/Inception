using UnityEngine;
using UnityEngine.Events;

public class HungerSystem : MonoBehaviour
{
    [SerializeField] private float hungerFactor = 0.3f;
    [SerializeField] private float starveInterval = 1;
    [SerializeField] private FilledBarUI saturationUI;
    [SerializeField] private UnityEvent onStarvationEvent;

    private float maxSaturation = 100;
    private float hungerAmount;

    public float MaxSaturation => maxSaturation;
    public float HungerAmount { get => hungerAmount; set => hungerAmount = Mathf.Clamp(value, 0, maxSaturation); }

    public static HungerSystem Instance { get; private set; }
    private bool isLoaded = false;
    private float starveTimer = 0;

    public void Initialize()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Load(PlayerData playerData)
    {
        maxSaturation = playerData.MaxSaturation;
        HungerAmount = playerData.Saturation;
        saturationUI.ChangeBarFill(HungerAmount, MaxSaturation);
        isLoaded = true;
    }

    public void StarveByTime(ClockContext context)
    {
        if (!isLoaded) return;

        HungerAmount -= Time.deltaTime * hungerFactor;

        if (HungerAmount == 0)
            Starve();
        
        saturationUI.ChangeBarFill(HungerAmount, MaxSaturation);
    }

    private void Starve()
    {
        if (starveTimer <= 0)
        {
            onStarvationEvent.Invoke();
            starveTimer = starveInterval;
        }
        starveTimer -= Time.deltaTime * hungerFactor;
    }

    public void RaiseSaturation(float saturation)
    {
        HungerAmount += saturation;
        saturationUI.ChangeBarFill(HungerAmount, MaxSaturation);
    }
}
