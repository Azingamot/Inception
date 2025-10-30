using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;
   
    public void SetCooldownFill(float fillAmount)
    {
        cooldownImage.fillAmount = Mathf.Clamp01(fillAmount);
    }
}
