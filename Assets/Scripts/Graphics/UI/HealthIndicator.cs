using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private Image healthBarImage;

    public void UpdateFill(float health, float maxHealth)
    {
        if (!healthBarObject.activeInHierarchy) healthBarObject.SetActive(true);
        else if (health == 0) healthBarObject.SetActive(false);
        healthBarImage.fillAmount = Mathf.Clamp01(health / maxHealth);
    }
}
